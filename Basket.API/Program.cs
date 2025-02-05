using Basket.API.Extensions;
using Basket.API.Extentions;
using Common.Logging;
using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
Log.Information("Starting Basket API up");

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
    Log.Information("Shut down Basket API complete");
    Log.CloseAndFlush();
}