using Serilog;
using Serilog.Formatting.Compact;

namespace WebAPi.Extensions;

public static class LoggerConfiguration
{
    public static void ConfigureLogs(this WebApplicationBuilder builder)
    {
        Log.Logger = new Serilog.LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console(new CompactJsonFormatter())
            .Enrich.FromLogContext()
            .ReadFrom.Configuration(builder.Configuration) // sobrescreve se existir no appsettings
            .CreateLogger();

        builder.Host.UseSerilog();
    }
}
