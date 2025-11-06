using AutoMapper;
using GQ.Common.Dto;
using GQ.Database;
using GQ.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GQ.Api.GraphQl;

public class Query()
{
    // Product Types
    public async Task<IEnumerable<ProductType>> GetProductTypes([FromServices] DataContext db)
    {
        var data = await db.ProductTypes.ToListAsync();

        return data;
        //return mapper.Map<List<ProductTypeDto>>(data);
    }

    // Product
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]   
    public async Task<IQueryable<Product>> GetProducts([FromServices] DataContext db)
    {
        var data = db.Products
            .Include(i=>i.ProductType)
            .AsQueryable();

        return data;
    }

    //  CCCCC
    public string Hello(string who) => $"Hello {who}!";
}