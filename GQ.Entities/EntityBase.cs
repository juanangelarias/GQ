using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GQ.Entities;

public interface IEntityBase : IConfigurableEntity
{
    long Id { get; set; }
    /*byte[] Timestamp { get; set; }*/
}

public class EntityBase
{
    public long Id { get; set; }
    /*public byte[] Timestamp { get; set; } = null!;*/

    protected void MapBaseEntityProperties<TEntity>(EntityTypeBuilder<TEntity> entity, string primaryKeyColumnName = "")
        where TEntity : class, IEntityBase
    {
        entity.ToTable(typeof(TEntity).Name);

        if (string.IsNullOrEmpty(primaryKeyColumnName))
        {
            entity.Property(p => p.Id)
                .HasColumnName(typeof(TEntity).Name + "Id")
                .ValueGeneratedOnAdd();
        }
        else
        {
            entity.Property(p => p.Id)
                .HasColumnName(primaryKeyColumnName)
                .ValueGeneratedNever();
        }

        /*entity.Property(p => p.Timestamp)
            .IsRowVersion();*/
    }
}