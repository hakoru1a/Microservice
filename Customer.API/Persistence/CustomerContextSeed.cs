using ILogger = Serilog.ILogger;

namespace Customer.API.Persistence
{
    public class CustomerContextSeed
    {
        public static async Task SeedCustomerAsync(CustomerContext customerContext, ILogger logger)
        {
            if (!customerContext.Customers.Any())
            {
                customerContext.AddRange(GetSeedCustomers());
                await customerContext.SaveChangesAsync();
                logger.Information("Seeded data for Customer DB associated with context {DbContextName}",
                    nameof(CustomerContext));
            }
        }

        private static IEnumerable<Entities.Customer> GetSeedCustomers()
        {
            return new List<Entities.Customer>
            {
                new Entities.Customer
                {
                    UserName = "john.doe",
                    FirstName = "John",
                    LastName = "Doe",
                    EmailAddress = "john.doe@example.com"
                },
                new Entities.Customer
                {
                    UserName = "jane.smith",
                    FirstName = "Jane",
                    LastName = "Smith",
                    EmailAddress = "jane.smith@example.com"
                },
                new Entities.Customer
                {
                    UserName = "robert.johnson",
                    FirstName = "Robert",
                    LastName = "Johnson",
                    EmailAddress = "robert.johnson@example.com"
                },
                new Entities.Customer
                {
                    UserName = "susan.wilson",
                    FirstName = "Susan",
                    LastName = "Wilson",
                    EmailAddress = "susan.wilson@example.com"
                }
            };
        }
    }
}