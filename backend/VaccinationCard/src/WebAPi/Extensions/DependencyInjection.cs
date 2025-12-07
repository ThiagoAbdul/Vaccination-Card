using Application;
using Application.Common.Interfaces;
using Application.Repositories;
using Application.Security;
using FluentValidation;
using Infrastructure.Context;
using Infrastructure.Interceptors.Database;
using Infrastructure.Message;
using Infrastructure.Repositories;
using Infrastructure.Security;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using System.Text;
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

        services.AddSecurity(configuration);

        return services;
    }

    private static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {

        var jwtConfig = configuration.GetSection("JwtSettings"); // Imagina que aqui ele pegou do Key Vault ou do AWS Parameter Store

        services
        .AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", async options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtConfig["Issuer"],
                ValidAudience = jwtConfig["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtConfig["Key"]!)
                )
            };
        });

        services.AddAuthorization();

        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyReference).Assembly); // Não ter que registrar os Hanlders no container de DI
        });

        services.AddValidatorsFromAssembly(typeof(ApplicationAssemblyReference).Assembly);

        return services;
    }

    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddEndpointsApiExplorer();

        services.AddHttpContextAccessor();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Vaccination Card API", Version = "v1" });

            // Define o esquema de segurança (Security Scheme)
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Insira o token JWT no campo 'Value' no formato: Bearer [seu_token]"
            });

            // Define o Requisito de Segurança (Security Requirement)
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });
        
        var allowedOrigins = configuration
        .GetSection("Cors:AllowedOrigins")
        .Get<string[]>() ?? Array.Empty<string>();

        services.AddCors(options =>
        {
            options.AddPolicy("DefaultPolicy", policy =>
            {
                policy
                    .WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });

        });



        return services;
    }
}

