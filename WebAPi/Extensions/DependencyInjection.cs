using Application;
using Application.Repositories;
using FluentValidation;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
namespace WebAPi.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
       
        services.AddDbContext<AppDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IVaccineRepository, VaccineRepository>();
        services.AddScoped<IVaccinationRepository, VaccinationRepository>();

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

