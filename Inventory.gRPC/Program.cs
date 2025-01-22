using Common.Logging;
using Inventory.gRPC.Extensitons;
using Inventory.gRPC.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(SeriLogger.Configure);


try
{
    // Add services to the container.
    builder.Services.AddGrpc();
    builder.Services.AddInfrastructure(builder.Configuration);

    var app = builder.Build();
    app.MapGrpcService<InventoryService>();
    // Configure the HTTP request pipeline.
    app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

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