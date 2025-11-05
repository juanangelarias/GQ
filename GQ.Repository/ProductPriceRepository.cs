using AutoMapper;
using GQ.Common.Dto;
using GQ.Database;
using GQ.Entities;

namespace GQ.Repository;

public interface IProductPriceRepository : IRepositoryBase<ProductPrice, ProductPriceDto>
{
}

public class ProductPriceRepository(IMapper mapper, DataContext db)
    : RepositoryBase<ProductPrice, ProductPriceDto>(mapper, db), IProductPriceRepository
{
}