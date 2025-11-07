using GQ.Database;
using GQ.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GQ.Api.GraphQl.Queries;

public partial class Query()
{
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]   
    public Task<IQueryable<Product>> GetProducts([FromServices] DataContext db)
    {
        var data = db.Products
            .Include(i => i.ProductType)
            .AsQueryable();

        return Task.FromResult(data);
    }
}