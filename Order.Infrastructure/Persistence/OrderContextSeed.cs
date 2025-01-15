using Microsoft.EntityFrameworkCore;
using ILogger = Serilog.ILogger;
using OrderCatalog = Order.Domain.Entities.Order;
namespace Order.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        private readonly ILogger _logger;
        private readonly OrderContext _context;

        public OrderContextSeed(OrderContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task InitialiseAsync()
        {
            try
            {
                if (_context.Database.IsSqlServer())
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, messageTemplate: "An error occurred while initialising the database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            if (!_context.Orders.Any())
            {
                await _context.Orders.AddRangeAsync(
                    new OrderCatalog
                    {
                        UserName = "customer1",
                        FirstName = "customer1",
                        LastName = "customer",
                        EmailAddress = "customer1@local.com",
                        ShippingAddress = "Wollongong",
                        InvoiceAddress = "Australia",
                        TotalPrice = 250
                    }); //Task
                await _context.SaveChangesAsync();
            }
        }
    }
}
