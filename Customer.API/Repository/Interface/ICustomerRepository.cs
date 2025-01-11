using Constracts.Common.Interface;
using Customer.API.Persistence;

namespace Customer.API.Repository.Interface
{
    public interface ICustomerRepository : IRepositoryBaseAsync<Entities.Customer, long, CustomerContext>
    {
    }
}
