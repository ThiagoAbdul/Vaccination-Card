using Application.Common.Enums;
using Application.Common.Interfaces;
using Application.Common.Models;

using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Interceptors.Database;

public class AuditInterceptor(IHttpContextAccessor httpContextAccessor, IMessageBus bus, IOptions<TopicsSettings> topicsSettings) : SaveChangesInterceptor
{
    private readonly TopicsSettings _topicsSettings = topicsSettings.Value;
    private static readonly JsonSerializerOptions _serializationOptions = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        WriteIndented = false
    };
    public override InterceptionResult<int> SavingChanges(
    DbContextEventData eventData,
    InterceptionResult<int> result)
    {
        AuditChangesAsync(eventData.Context).Wait(); // Evitar SaveChanges() -> Pode causar até deadlocks
        return base.SavingChanges(eventData, result);
    }

    public async override ValueTask<InterceptionResult<int>> SavingChangesAsync(
    DbContextEventData eventData,
    InterceptionResult<int> result,
    CancellationToken cancellationToken = default)
    {
        await AuditChangesAsync(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task AuditChangesAsync(DbContext? context)
    {
        if (context is null) return;

        var entries = context.ChangeTracker.Entries<IAuditable>()
            .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted);

        List<AuditLog> logs = new (entries.Count());

        foreach ( var entry in entries )
        {
            if(entry.State is EntityState.Added)
            {
                entry.Entity.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
                entry.Entity.CreatedById = null; // TODO
                entry.Entity.CreatedByName = null; // TODO
            }
            else
            {
                entry.Entity.LastUpdatedAt = DateOnly.FromDateTime(DateTime.Now);
                entry.Entity.LastUpdatedById = null; // TODO
                entry.Entity.LastUpdatedByName = null; // TODO
            }

            var log = GetAuditLog(entry);
            logs.Add(log); 
        }

        await bus.PublishRangeAsync(_topicsSettings.Audit, logs);
    }

    private AuditLog GetAuditLog(EntityEntry entry)
    {
        string table = entry.Entity.GetType().Name;
        var log = new AuditLog
        {
            Timestamp = DateTime.UtcNow,
            RefrredTable = table,
        };
        if (entry.State == EntityState.Added)
        {
            log.Operation = CrudOperation.CREATE;
            log.CurrentJsonSnapshot = JsonSerializer.Serialize(entry.Entity, _serializationOptions);
        }
        if (entry.State == EntityState.Modified)
        {
            log.Operation = CrudOperation.UPDATE;
            log.CurrentJsonSnapshot = JsonSerializer.Serialize(entry.Entity, _serializationOptions);
            var previousState = entry.OriginalValues.ToObject();
            log.PreviousJsonSnapshot = JsonSerializer.Serialize(previousState, _serializationOptions);
        }


        string rowsIds = entry.Metadata.FindPrimaryKey()?.Properties
            .Select(p => entry.Property(p.Name).CurrentValue?.ToString())
            .Aggregate((acc, next) => $"{acc},{next}")
            ?? string.Empty;

        log.RowIds = rowsIds;


        log.ClientIp = GetClientIp();

        return log;
    }

    private string GetClientIp()
    {
        var httpContext = httpContextAccessor.HttpContext;

        if (httpContext == null) return string.Empty;

        string? clientIp = httpContext.Request.Headers["X-Forwarded-For"];

        return clientIp
            ?? httpContext.Connection.RemoteIpAddress?.ToString()
            ?? string.Empty;
    }
}
