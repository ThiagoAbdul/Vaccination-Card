using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WebAPi.Endpoints;
using WebAPi.Extensions;
using WebAPi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogs();


builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication(builder.Configuration)
    .AddPresentation(builder.Configuration);


builder.Services.AddHealthChecks();

var app = builder.Build();


app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
    }


    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHealthChecks("/health");

app.UseSerilogRequestLogging();


app.UseCors("DefaultPolicy");

app.UseAuthentication();
app.UseAuthorization();


app
    .MapPersonEndpoints()
    .MapVaccineEndpoints()
    .MapVaccinationEndpoints();

app.Run();
