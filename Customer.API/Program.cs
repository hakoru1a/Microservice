using Common.Logging;
using Customer.API.Extensions;
using Customer.API.Extentions;
using Customer.API.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// start up log
Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
Log.Information("Starting Customer API up");

try
{
    builder.Host.UseSerilog(SeriLogger.Configure);
    builder.Host.AddAppConfigurations();
    builder.Services.AddInfrastructure(builder.Configuration);
    var app = builder.Build();
    app.UseInfrastructure();
    app.MapControllers();
    app.MapEndpoints();
    app.MigrateDatabase<CustomerContext>((context, _) =>
    {
        CustomerContextSeed.SeedCustomerAsync(context, Log.Logger).Wait();
    }).Run();
}
catch (HostAbortedException ex)
{
    // Xử lý riêng cho HostAbortedException
    Log.Warning("Host was aborted during startup: {Message}", ex.Message);
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandle Exception");
}
finally
{
    Log.Information("Shut down Product API complete");
    Log.CloseAndFlush();
}