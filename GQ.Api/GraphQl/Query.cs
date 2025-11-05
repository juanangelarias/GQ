using AutoMapper;
using GQ.Common.Dto;
using GQ.Database;
using GQ.Entities;
using GQ.Repository;
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
    public async Task<IEnumerable<Product>> GetProducts([FromServices] DataContext db)
    {
        var data = await db.Products
            .Include(i=>i.ProductType)
            .ToListAsync();

        return data;
    }

    //  CCCCC
    public string Hello(string who) => $"Hello {who}!";
}