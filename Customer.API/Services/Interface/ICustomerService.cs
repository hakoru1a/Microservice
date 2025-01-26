using Shared.DTOs.Customer;

namespace Customer.API.Services.Interface
{
    public interface ICustomerService
    {
        Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto customer);
        Task<CustomerDto> GetCustomerByIdAsync(long id);
        Task<CustomerDto?> GetCustomerByUsername(string username);
        Task<List<CustomerDto>> GetCustomers(long id);
        Task<CustomerDto> GetCustomerByEmailAddressAsync(string email);
        Task<CustomerDto> UpdateCustomerAsync(long id, UpdateCustomerDto customer);
        Task<bool> DeleteCustomerAsync(long id);
    }
}
