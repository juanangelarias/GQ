using GQ.Database;
using GQ.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GQ.Api.GraphQl.Queries;

public partial class Query
{ 
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]  
    public Task<IQueryable<ProductType>> GetProductTypes([FromServices] DataContext db)
    {
        var data = db.ProductTypes
            .OrderBy(o=>o.Name)
            .AsQueryable();

        return Task.FromResult(data);
    }
}