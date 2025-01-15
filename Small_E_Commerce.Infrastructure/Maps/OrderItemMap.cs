using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Small_E_Commerce.Orders;
using Small_E_Commerce.Orders.OrderItems;

namespace Small_E_Commerce.Infrastructure.Maps;

public class OrderItemMap : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.OrderId)
            .HasColumnName("OrderId")
            .IsRequired();

        builder.Property(oi => oi.ProductId)
            .HasColumnName("ProductId")
            .IsRequired();

        builder.Property(oi => oi.Quantity)
            .HasColumnName("Quantity")
            .IsRequired();

        builder.Property(oi => oi.Price)
            .HasColumnName("Price")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(oi => oi.TotalPrice)
            .HasColumnName("TotalPrice")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(oi => oi.SubOrderId)
            .HasColumnName("SubOrderId")
            .HasMaxLength(100)
            .IsRequired();
        
        builder.HasIndex(oi => oi.ProductId);

        builder.Property(x => x.Id).UseHiLo();
    }
}