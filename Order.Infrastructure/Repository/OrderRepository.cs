using Constracts.Common.Interface;
using Infrastructure.Common.Repository;
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

        public void CreateOrder(OrderCatalog order) => Create(order);
      

        public void DeleteOrder(OrderCatalog order) => Delete(order);


        public Task<OrderCatalog> GetOrderByDocumentNo(string documentNo)
           => FindByCondition(x => x.No.ToString().Equals(documentNo)).FirstOrDefaultAsync();

        public async Task<IEnumerable<OrderCatalog>> GetOrdersByUsername(string username)
            => await FindByCondition(x => x.UserName == username).ToListAsync();
    }
}
