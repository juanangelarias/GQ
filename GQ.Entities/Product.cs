using Microsoft.EntityFrameworkCore;

namespace GQ.Entities;

public class Product: EntityBase, IEntityBase
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public long ProductTypeId { get; set; }
    
    //

    public ProductType ProductType { get; set; } = null!;
    public virtual List<ProductPrice> Prices { get; set; } = [];
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<Product>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.Code)
                .HasMaxLength(20)
                .IsRequired();

            e.Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            e.Property(p => p.ProductTypeId)
                .IsRequired();

            e.HasIndex(i => i.Code)
                .IsUnique();

            e.HasIndex(i => i.Name);

            e.HasOne(o => o.ProductType)
                .WithMany()
                .HasForeignKey(o => o.ProductTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasMany(x => x.Prices)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}