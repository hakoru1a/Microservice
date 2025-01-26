using Common.Logging;
using Ocelot.API.Extensions;
using OcelotAPIGateway.Extensions;
using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
Log.Information("Starting Ocelot API up");

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog(SeriLogger.Configure);
    builder.Host.AddAppConfigurations();

    builder.Services.AddInfrastructure(builder.Configuration);

    var app = builder.Build();



    app.MapControllers();

    app.UseInfrastructure();


    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandle Exception");
}
finally
{
    Log.Information("Shut down Starting API complete");
    Log.CloseAndFlush();
}