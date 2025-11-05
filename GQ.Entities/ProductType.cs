using Microsoft.EntityFrameworkCore;

namespace GQ.Entities;

public class ProductType: EntityBase, IEntityBase
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<ProductType>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.Code)
                .HasMaxLength(10)
                .IsRequired();

            e.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

            e.HasIndex(p => p.Code)
                .IsUnique();

            e.HasIndex(p => p.Name);
        });
    }
}