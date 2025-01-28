using Hangfire;
using Hangfire.API.Extensions;
using Infrastructure.ScheduleJob;
using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
Log.Information("Starting Hangfire up");

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.AddAppConfigurations();
    builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.Services.AddControllers();
    builder.Services.AddHangfireService();
    var app = builder.Build();

    
    app.UseRouting();

    app.UseAuthorization();

    app.UseHangfireDashboard(builder.Configuration);

    app.UseInfrastructure();


    app.UseEndpoints(endpoints =>
    {
        endpoints.MapDefaultControllerRoute();
        endpoints.MapGet("/", () => Results.Redirect("/jobs"));
    });

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandle Exception");
}
finally
{
    Log.Information("Shut down Hangfire complete");
    Log.CloseAndFlush();
}