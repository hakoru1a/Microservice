using Infrastructure.Middlewares;
using Ocelot.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OcelotAPIGateway.Extensions
{
    public static class ApplicationExtensions
    {
        public static async Task<IApplicationBuilder> UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseSwagger()
               .UseSwaggerForOcelotUI(option =>
               {
                   option.PathToSwaggerGenerator = "/swagger/docs";
               })
               .UseRouting()
               .UseMiddleware<ErrorWrappingMiddleware>()
               .UseAuthentication()
               .UseAuthorization()
               .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers(); // Ánh xạ các controller nội bộ
                    endpoints.MapGet("/", async context =>
                    {
                        context.Response.Redirect("/swagger/index.html");
                    });
                })
               .UseCors("CorsPolicy");

            await app.UseOcelot();
            return app;
        }
    }

}