using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Interceptors.Database;

public class LoggerInterceptor(ILogger<LoggerInterceptor> logger) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
    DbContextEventData eventData,
    InterceptionResult<int> result)
    {
        LogChanges(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
    DbContextEventData eventData,
    InterceptionResult<int> result,
    CancellationToken cancellationToken = default)
    {
        LogChanges(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void LogChanges(DbContext? context)
    {
        if (context == null)
            return;

        var entries = context.ChangeTracker
                .Entries()
                .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted);

        foreach (var entry in entries)
        {
            string entityName = entry.Entity.GetType().Name;
            string operation = entry.State switch
            {
                EntityState.Added => "INSERT",
                EntityState.Modified => "UPDATE",
                EntityState.Deleted => "DELETE",
                _ => "UNKNOWN"
            };

            logger.LogInformation(
                "EF Operation: {Operation} on entity {Entity}. Keys: {Keys}",
                operation,
                entityName,
                GetPrimaryKeys(entry)
            );
        }
    }

    private static object? GetPrimaryKeys(EntityEntry entry)
    {
        var keyProps = entry.Metadata.FindPrimaryKey()?.Properties;
        if (keyProps == null) return null;

        var dict = new Dictionary<string, object?>();
        foreach (var prop in keyProps)
        {
            dict[prop.Name] = entry.Property(prop.Name).CurrentValue;
        }

        return dict;
    }
}
