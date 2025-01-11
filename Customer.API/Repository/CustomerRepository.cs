using Constracts.Common.Interface;
using Customer.API.Persistence;
using Customer.API.Repository.Interface;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Customer.API.Repository
{
    public class CustomerRepository : RepositoryBaseAsync<Entities.Customer, long, CustomerContext>, ICustomerRepository
    {
        public CustomerRepository(CustomerContext dbContext, IUnitOfWork<CustomerContext> unitOfWork) : base(dbContext, unitOfWork)
        {
        }

    }
}
