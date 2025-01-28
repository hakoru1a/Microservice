using Constracts.ScheduleJob;
using Constracts.Services;
using Hangfire.API.Services;
using Hangfire.API.Services.Interfaces;
using Infrastructure.Configurations;
using Infrastructure.Extensions;
using Infrastructure.ScheduleJob;
using Infrastructure.Services;
using Microsoft.OpenApi.Models;
using Shared.Configurations;

namespace Hangfire.API.Extensions
{
    public static class ServiceExtensions
    {
        internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
            IConfiguration configuration)
        {
            var hangFireSettings = configuration.GetSection(key: nameof(HangFireSettings))
                .Get<HangFireSettings>();
            services.AddSingleton(hangFireSettings);
            var emailSettings = services.GetOptions<SMTPEmailSettings>(sectionName: nameof(SMTPEmailSettings));
            services.AddSingleton(emailSettings);
            services.AddTransient<IScheduleJobService, HangfireService>();
            services.AddTransient<ISMTPEmailServices, SMTTEmailServices>();
            services.AddTransient<IBackgroundJobService, BackgroundJobService>();
            ConfigureSwagger(services);
            return services;
        }

        private static void ConfigureSwagger(this IServiceCollection services)
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Ecom API",
                        Version = "v1",
                        Description = "E-commerce API for Customer Management",  // Added meaningful description
                        Contact = new OpenApiContact
                        {
                            Name = "Chuong Dang",
                            Email = "hakoru1a@gmail.com",
                        }
                    });
                });
            }
        } 
}
