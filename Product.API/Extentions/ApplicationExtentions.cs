namespace Product.API.Extentions
{
    public static class ApplicationExtentions
    {
        public static void UseInfrastructure(this IApplicationBuilder app) 
        {
            app.UseAuthorization();
        }
    }
}
