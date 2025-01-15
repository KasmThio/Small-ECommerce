using Dawn;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Small_E_Commerce.Internals;

public  class AggregateBase<TId> where TId : struct
{
    public TId Id { get; private set; }
    public Guid CreatedBy { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public Guid UpdatedBy { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public int Version { get; private set; }

    public AggregateBase()
    {

    }

    public AggregateBase(TId id, Guid createdBy, DateTimeOffset createdAt, Guid updatedBy, DateTimeOffset updatedAt)
    {
        Id = Guard.Argument(id, nameof(id)).NotDefault();
        CreatedBy = Guard.Argument(createdBy, nameof(createdBy)).NotDefault();
        CreatedAt = Guard.Argument(createdAt, nameof(createdAt)).NotDefault();
        UpdatedBy = Guard.Argument(updatedBy, nameof(updatedBy)).NotDefault();
        UpdatedAt = Guard.Argument(updatedAt, nameof(updatedAt)).NotDefault();
    }


    public void SetCreatedAt(DateTimeOffset createdAt)
    {
        CreatedAt = Guard.Argument(createdAt, nameof(createdAt)).NotDefault();
    }
    
    public void SetUpdatedAt(DateTimeOffset updatedAt)
    {
        UpdatedAt = Guard.Argument(updatedAt, nameof(updatedAt)).NotDefault();
    }
}


public abstract class EntityBase<TId> where TId : struct
{
    public TId Id { get; private set; }

    public EntityBase()
    {

    }

    public EntityBase(TId id)
    {
        this.Id = id;
    }
}


