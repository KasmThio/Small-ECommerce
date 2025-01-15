using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Small_E_Commerce.Internals;

namespace Small_E_Commerce.Infrastructure.Maps;

public abstract class AggregateConfigurationMap<TAggregate, TId> : IEntityTypeConfiguration<TAggregate>
    where TAggregate : AggregateBase<TId>
    where TId : struct
{
    public virtual void Configure(EntityTypeBuilder<TAggregate> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
        builder.Property(x => x.CreatedAt).HasColumnName("CreatedAt");
        builder.Property(x => x.UpdatedBy).HasColumnName("UpdatedBy");
        builder.Property(x => x.UpdatedAt).HasColumnName("UpdatedAt");
    }
}

public abstract class EntityConfigurationMap<TEntity, TId> : IEntityTypeConfiguration<TEntity>
    where TEntity : EntityBase<TId>
    where TId : struct
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(x => x.Id);
    }
}