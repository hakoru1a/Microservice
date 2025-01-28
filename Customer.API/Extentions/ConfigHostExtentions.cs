using Customer.API.Extensions;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Shared.Configurations;

namespace Customer.API.Extentions
{
    public static class ConfigHostExtentions
    {
        public static void AddAppConfigurations(this ConfigureHostBuilder host)
        {
            host.ConfigureAppConfiguration(configureDelegate: (context, config) =>
            {
                IHostEnvironment env = context.HostingEnvironment;
                config.AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                      .AddJsonFile(path: $"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                      .AddEnvironmentVariables();
            });


        }
        internal static IApplicationBuilder UseHangfireDashboard(this IApplicationBuilder app, IConfiguration configuration)
        {
            var configDashboard = configuration.GetSection(key: "HangFireSettings:Dashboard").Get<DashboardOptions>();
            var hangfireSettings = configuration.GetSection(key: "HangFireSettings").Get<HangFireSettings>();
            var hangfireRoute = hangfireSettings.Route;

            app.UseHangfireDashboard(hangfireRoute, new DashboardOptions
            {
                Authorization = new[] { new AuthorizationFilter()},
                DashboardTitle = configDashboard.DashboardTitle,
                StatsPollingInterval = configDashboard.StatsPollingInterval,
                AppPath = configDashboard.AppPath,
                IgnoreAntiforgeryToken = true
            });

            return app;
        }
        public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();

                try
                {
                    logger.LogInformation(message: "Migrating postgre database.");

                    ExecuteMigrations(context);

                    logger.LogInformation(message: "Migrated postgre database.");

                    InvokeSeeder(seeder, context, services);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, message: "An error occurred while migrating the postgre database");
                }
            }

            return host;
        }

        private static void ExecuteMigrations<TContext>(TContext context) where TContext : DbContext
        {
            context.Database.Migrate();
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services) where TContext : DbContext
        {
            seeder(context, services);
        }
    }
}
