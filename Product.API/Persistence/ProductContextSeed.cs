using Product.API.Entities;
using ILogger = Serilog.ILogger;

namespace Product.API.Persistence
{
    public class ProductContextSeed
    {
        public static async Task SeedProductAsync(ProductContext productContext, ILogger logger)
        {

            if (!productContext.Products.Any())
            {
                productContext.AddRange(entities: getCatalogProducts());
                await productContext.SaveChangesAsync();
                logger.Information("Seeded data for Product DB associated with context {DbContextName}",
                    nameof(ProductContext));
            }
        }

        private static IEnumerable<CatalogProduct> getCatalogProducts()
        {
            return new List<CatalogProduct>
            {
                new CatalogProduct
                {
                    No = "Prod-1",
                    Name = "iPhone 14",
                    Summary = "Latest iPhone model",
                    Description = "Latest iPhone with advanced features",
                    Price = 999.99m
                },
                new CatalogProduct
                {
                    No = "Prod-2",
                    Name = "Samsung Galaxy S23",
                    Summary = "Flagship Android phone",
                    Description = "High-end Android smartphone with premium features",
                    Price = 899.99m
                }
            };
        }
    }
}