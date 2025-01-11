using AutoMapper;
using Shared.DTOs.Customer;
using Infrastructure.Mapping;

namespace Customer.API
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Entities.Customer, CustomerDto>();
            CreateMap<CreateCustomerDto, Entities.Customer>();
            CreateMap<UpdateCustomerDto, Entities.Customer>()
                .IgnoreAllNonExisting();
        }
    }
}