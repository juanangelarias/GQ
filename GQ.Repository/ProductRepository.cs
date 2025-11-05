using AutoMapper;
using GQ.Common.Dto;
using GQ.Database;
using GQ.Entities;

namespace GQ.Repository;

public interface IProductRepository : IRepositoryBase<Product, ProductDto>
{
}

public class ProductRepository(IMapper mapper, DataContext db)
    : RepositoryBase<Product, ProductDto>(mapper, db), IProductRepository
{
}