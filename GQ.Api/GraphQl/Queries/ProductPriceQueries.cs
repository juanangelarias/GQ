using GQ.Database;
using GQ.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GQ.Api.GraphQl.Queries;

public partial class Query
{
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public Task<IQueryable<ProductPrice>> GetProductPrices([FromServices] DataContext db)
    {
        var data = db.ProductPrices
            .OrderBy(o => o.ProductId)
            .ThenByDescending(o => o.Date)
            .AsQueryable();

        return Task.FromResult(data);
    }

    public async Task<decimal> GetProductPriceByProduct([FromServices] DataContext db, int productId)
    {
        var data = await db.ProductPrices
            .OrderBy(o => o.Date)
            .FirstOrDefaultAsync(o => o.ProductId == productId);

        return data?.Price ?? 0;
    }
}