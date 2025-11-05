using AutoMapper;
using GQ.Common.Dto;
using GQ.Entities;

namespace GQ.Database.Mappings;

public class SqlMappingsProfile: Profile
{
    public SqlMappingsProfile()
    {
        // P
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<ProductPrice, ProductPriceDto>().ReverseMap();
        CreateMap<ProductType, ProductTypeDto>().ReverseMap();
    }
}