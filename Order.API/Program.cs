using Common.Logging;
using Serilog;
using Order.Infrastructure;
using Order.Infrastructure.Persistence;
using Order.Application;
using Ordering.API.Extensions;
using Order.API.Extenstions;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
Log.Information("Starting Order API up");

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog(SeriLogger.Configure);
    builder.Host.AddAppConfigurations();


    builder.Services.AddInfrastructure(builder.Configuration)
                    .AddConfigurationSettings(builder.Configuration)
                    .AddApplicationServices();

    builder.Services.AddControllers();
    var app = builder.Build();

    // Initialise and seed database
    using (var scope = app.Services.CreateScope())
    {
        var orderContextSeed = scope.ServiceProvider.GetRequiredService<OrderContextSeed>();
        await orderContextSeed.InitialiseAsync();
        await orderContextSeed.SeedAsync();
    }

    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        // Change the Swagger endpoint
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

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
    Log.Information("Shut down Order API complete");
    Log.CloseAndFlush();
}
//dotnet ef migrations add "Init_OrderDB" --project Order.Infrastructure --startup-project Order.API --output-dir Persistence\Migrations

//dotnet ef database update --project Order.Infrastructure --startup-project Order.API

//dotnet ef migrations add "Add_Status" --project Order.Infrastructure --startup-project Order.API --output-dir Persistence\Migrations