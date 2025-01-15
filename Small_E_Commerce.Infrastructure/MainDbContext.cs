using Microsoft.EntityFrameworkCore;
using Small_E_Commerce.Orders.OrderItems;

namespace Small_E_Commerce.Infrastructure;


public class MainDbContext(DbContextOptions<MainDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainDbContext).Assembly);

        modelBuilder.HasSequence<long>("SubOrderIdSequence")
            .IncrementsBy(10);

        modelBuilder.Entity<OrderItem>()
            .Property(oi => oi.Id)
            .UseHiLo("SubOrderIdSequence");

        base.OnModelCreating(modelBuilder);
    }
}
