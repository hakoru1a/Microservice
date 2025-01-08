using AutoMapper;
using Product.API.Entities;
using Shared.DTOs.Product;
using Infrastructure.Mapping;

namespace Product.API
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CatalogProduct, ProductDto>();
            CreateMap<CreateProductDto, CatalogProduct>();
            CreateMap<UpdateProductDto, CatalogProduct>()
                .IgnoreAllNonExisting();
        }
    }
}