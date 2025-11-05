using Microsoft.EntityFrameworkCore;

namespace GQ.Entities;

public class ProductPrice: EntityBase, IEntityBase
{
    public long ProductId { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public decimal Price { get; set; }
    
    //

    public Product Product { get; set; } = null!;
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<ProductPrice>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.ProductId)
                .IsRequired();

            e.Property(p => p.Date)
                .IsRequired();

            e.Property(p => p.Price)
                .IsRequired();

            e.HasIndex(i => new { i.ProductId, i.Date })
                .IsDescending(false, true)
                .IsUnique();
        });
    }
}