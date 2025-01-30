using Common.Logging;
using Saga.Orchestrator.Extensions;
using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
Log.Information("Starting Saga Orchesstrator API up");

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog(SeriLogger.Configure);

    builder.Host.AddAppConfigurations();
    builder.Services.AddInfrastructure(builder.Configuration);
    var app = builder.Build();
    app.UseInfrastructure();
    app.MapControllers();
    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandle Exception");
}
finally
{
    Log.Information("Shut down Saga Orchesstrator API complete");
    Log.CloseAndFlush();
}