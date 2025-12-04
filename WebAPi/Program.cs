using Serilog;
using WebAPi.Endpoints;
using WebAPi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogs();


builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddPresentation();


builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHealthChecks("/health");

app.UseSerilogRequestLogging();

app
    .MapPersonEndpoints()
    .MapVaccineEndpoints()
    .MapVaccinationEndpoints();

app.Run();
