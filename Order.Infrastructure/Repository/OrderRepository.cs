using Constracts.Common.Interface;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Order.Domain.Interfaces;
using Order.Infrastructure.Persistence;
using OrderCatalog = Order.Domain.Entities.Order;
namespace Order.Infrastructure.Repository
{
    public class OrderRepository : RepositoryBaseAsync<OrderCatalog, long, OrderContext>, IOrderRepository
    {
        public OrderRepository(OrderContext dbContext, IUnitOfWork<OrderContext> unitOfWork) : base(dbContext, unitOfWork)
        {
        }
        public async Task<IEnumerable<OrderCatalog>> GetOrdersByUsername(string username)
            => await FindByCondition(x => x.UserName == username).ToListAsync();
    }
}
