using Common.Logging;
using HealthChecks.UI.Client;
using Inventory.gRPC.Extensitons;
using Inventory.gRPC.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(SeriLogger.Configure);

try
{
    // Add services to the container.
    builder.Services.AddGrpc();
    builder.Services.AddInfrastructure(builder.Configuration);

    var app = builder.Build();
    app.UseRouting();

    app.UseEndpoints(endpoints =>
    {
        // health checks
        endpoints.MapHealthChecks(pattern:"/hc", new HealthCheckOptions()
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        endpoints.MapGrpcHealthChecksService();

        endpoints.MapGrpcService<InventoryService>();

        endpoints.MapGet(pattern:"/", async context =>
        {
            await context.Response.WriteAsync("Communication with gRPC endpoints must be made through");
        });
    });

    app.Run();

}
catch (Exception ex)
{
    Log.Error("Error in start up");
}
finally 
{
    Log.Information("Shut down Inventory.gRPC");
}