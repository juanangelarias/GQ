using AutoMapper;
using GQ.Common.Dto;
using GQ.Database;
using GQ.Entities;

namespace GQ.Repository;

public interface IProductTypeRepository : IRepositoryBase<ProductType, ProductTypeDto>
{
}

public class ProductTypeRepository(IMapper mapper, DataContext db)
    : RepositoryBase<ProductType, ProductTypeDto>(mapper, db), IProductTypeRepository
{
}