using GQ.Common.Exceptions;
using GQ.Database;
using GQ.Entities;
using HotChocolate.Subscriptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GQ.Api.GraphQl.Mutations;

public partial class Mutation
{
    public async Task<Product> SetProduct(long id, string code, string name, long productTypeId,
        [FromServices] DataContext db, [Service] ITopicEventSender eventSender, CancellationToken cancellationToken)
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

    public async Task<bool> DeleteProduct(long id, [FromServices] DataContext db)
    {
        var product = await db.Products
            .FirstOrDefaultAsync(f => f.Id == id);

        if (product == null)
            throw new NotFoundException("Product not found");

        db.Products.Remove(product);
        await db.SaveChangesAsync();

        return true;
    }
}