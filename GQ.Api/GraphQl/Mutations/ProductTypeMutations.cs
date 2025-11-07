using GQ.Common.Exceptions;
using GQ.Database;
using GQ.Entities;
using HotChocolate.Subscriptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GQ.Api.GraphQl.Mutations;

public partial class Mutations
{
    public async Task<ProductType> SetProductType(long id, string code, string name, [FromServices] DataContext db,
        [Service] ITopicEventSender eventSender, CancellationToken cancellationToken)
    {
        var productType = new ProductType
        {
            Id = id,
            Code = code,
            Name = name
        };

        if (productType.Id == 0)
        {
            db.ProductTypes.Add(productType);
        }
        else
        {
            var dbProductType = await db.ProductTypes
                .FirstOrDefaultAsync(f => f.Id == productType.Id, cancellationToken: cancellationToken);

            if (dbProductType != null)
                db.ProductTypes.Update(productType);
            else
                throw new NotFoundException("Product Type not found");
        }

        await db.SaveChangesAsync(cancellationToken);

        await eventSender.SendAsync(nameof(ProductType), productType, cancellationToken);

        return productType;
    }

    public async Task<bool> DeleteProductType(long id, [FromServices] DataContext db)
    {
        var productType = await db.ProductTypes
            .FirstOrDefaultAsync(f=>f.Id == id);
        
        if(productType == null)
            throw new NotFoundException("Product Type not found");
        
        db.ProductTypes.Remove(productType);
        await db.SaveChangesAsync();
        
        return true;   
    }
}