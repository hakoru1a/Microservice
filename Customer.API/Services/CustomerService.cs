using AutoMapper;
using Customer.API.Repository.Interface;
using Customer.API.Services.Interface;
using Shared.DTOs.Customer;
using Microsoft.EntityFrameworkCore;

namespace Customer.API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto customer)
        {
            var EmailAddressExists = await _repository.FindByCondition(x => x.EmailAddress == customer.EmailAddress).AnyAsync();
            if (EmailAddressExists)
                throw new Exception($"Customer with EmailAddress {customer.EmailAddress} already exists.");

            var entity = _mapper.Map<Entities.Customer>(customer);
            var id = await _repository.CreateAsync(entity);
            await _repository.SaveChangesAsync();

            var createdCustomer = await _repository.GetByIdAsync(id);
            return _mapper.Map<CustomerDto>(createdCustomer);
        }

        public async Task<bool> DeleteCustomerAsync(long id)
        {
            var customer = await _repository.GetByIdAsync(id);
            if (customer == null) return false;

            await _repository.DeleteAsync(customer);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<CustomerDto> GetCustomerByEmailAddressAsync(string EmailAddress)
        {
            var customer = await _repository.FindByCondition(x => x.EmailAddress == EmailAddress)
                                          .FirstOrDefaultAsync();
            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task<CustomerDto> GetCustomerByIdAsync(long id)
        {
            var customer = await _repository.GetByIdAsync(id);
            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task<List<CustomerDto>> GetCustomers(long id)
        {
            var customers = await _repository.FindAll()
                                           .ToListAsync();
            return _mapper.Map<List<CustomerDto>>(customers);
        }

        public async Task<CustomerDto> UpdateCustomerAsync(long id, UpdateCustomerDto customerDto)
        {
            var customer = await _repository.GetByIdAsync(id);
            if (customer == null)
                throw new Exception($"Customer with id {id} not found.");

            if (customerDto.EmailAddress != customer.EmailAddress)
            {
                var EmailAddressExists = await _repository.FindByCondition(x => x.EmailAddress == customerDto.EmailAddress).AnyAsync();
                if (EmailAddressExists)
                    throw new Exception($"Customer with EmailAddress {customerDto.EmailAddress} already exists.");
            }

            _mapper.Map(customerDto, customer);
            await _repository.UpdateAsync(customer);
            await _repository.SaveChangesAsync();

            return _mapper.Map<CustomerDto>(customer);
        }
    }
}