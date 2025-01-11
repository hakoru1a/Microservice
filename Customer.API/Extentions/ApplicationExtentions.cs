using Customer.API.Controllers;

namespace Customer.API.Extentions
{
    public static class ApplicationExtentions
    {
        public static void UseInfrastructure(this IApplicationBuilder app) 
        {
            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                // Change the Swagger endpoint
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
        }

        public static void MapEndpoints(this WebApplication app)
        {
            app.MapCustomerEndpoints();
        }

    }
}
