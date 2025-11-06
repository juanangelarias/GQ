using GQ.Common.Exceptions;
using GQ.Database;
using GQ.Entities;
using HotChocolate.Subscriptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GQ.Api.GraphQl;

public class Mutation
{
    public async Task<Product> SetProduct(long id, string code, string name, long productTypeId,
        [FromServices] DataContext db,
        [Service] ITopicEventSender eventSender, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Id = id,
            Code = code,
            Name = name,
            ProductTypeId = productTypeId,
        };

        if (product.Id == 0)
        {
            db.Products.Add(product);
        }
        else
        {
            var dbProduct = await db.Products
                .FirstOrDefaultAsync(f => f.Id == product.Id, cancellationToken: cancellationToken);
            
            if (dbProduct != null)
                db.Products.Update(product);
            else
                throw new NotFoundException("Product not found");
        }

        await db.SaveChangesAsync(cancellationToken);

        await eventSender.SendAsync(nameof(Product), product, cancellationToken);

        return product;
    }

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
}