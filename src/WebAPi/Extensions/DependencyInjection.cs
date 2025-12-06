using Application;
using Application.Common.Interfaces;
using Application.Repositories;
using Application.Security;
using FluentValidation;
using Infrastructure.Context;
using Infrastructure.Interceptors;
using Infrastructure.Interceptors.Database;
using Infrastructure.Message;
using Infrastructure.Repositories;
using Infrastructure.Security;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
namespace WebAPi.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services.Configure<TopicsSettings>(configuration.GetSection("Topics"));

        services.AddScoped<LoggerInterceptor>();
        services.AddScoped<AuditInterceptor>();

        services.AddHttpContextAccessor();
       
        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            var logger = sp.GetRequiredService<LoggerInterceptor>();
            var audit = sp.GetRequiredService<AuditInterceptor>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options
            .UseNpgsql(connectionString)
            .AddInterceptors(logger, audit);
        });

        services.AddSingleton<IVaultClient, MockVaultClient>();
        services.AddSingleton<IHashService, HashService>();

        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IVaccineRepository, VaccineRepository>();
        services.AddScoped<IVaccinationRepository, VaccinationRepository>();

        services.AddScoped<IMessageBus, MockMessageBus>();

        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyReference).Assembly); // Não ter que registrar os Hanlders no container de DI
        });

        services.AddValidatorsFromAssembly(typeof(ApplicationAssemblyReference).Assembly);

        return services;
    }

    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();


        return services;
    }
}

