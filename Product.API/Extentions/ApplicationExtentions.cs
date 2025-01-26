using Infrastructure.Middlewares;

namespace Product.API.Extentions
{
    public static class ApplicationExtentions
    {
        public static void UseInfrastructure(this IApplicationBuilder app) 
        {

            app.UseSwagger();

            app.UseMiddleware<ErrorWrappingMiddleware>();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwaggerUI(options =>
            {
                // Change the Swagger endpoint
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
        }
    }
}
