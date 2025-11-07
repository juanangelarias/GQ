using GQ.Common.Exceptions;
using GQ.Database;
using GQ.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GQ.Api.GraphQl.Mutations;

public partial class Mutations
{
    public ProductPrice SetProductPrice(long productId, decimal price, [FromServices] DataContext db)
    {
        var productPrice = new ProductPrice
        {
            Id = 0,
            ProductId = productId,
            Date = DateTime.Now,
            Price = price
        };

        db.ProductPrices.Add(productPrice);
        db.SaveChanges();

        return productPrice;
    }

    public async Task<bool> DeleteProduct(long id, [FromServices] DataContext db)
    {
        var productPrice = await db.ProductPrices
            .FirstOrDefaultAsync(f=>f.Id == id);
        
        if(productPrice == null)
            throw new NotFoundException("Product Price not found");

        db.ProductPrices.Remove(productPrice);
        await db.SaveChangesAsync();
        
        return true;
    }
}