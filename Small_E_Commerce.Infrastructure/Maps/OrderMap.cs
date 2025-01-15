using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Small_E_Commerce.Orders;

namespace Small_E_Commerce.Infrastructure.Maps;

public class OrderMap : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);
        
        builder.Property(o => o.OrderIdentifier)
            .HasColumnName("OrderIdentifier")
            .IsRequired();

        builder.Property(o => o.TotalPrice)
            .HasColumnName("TotalPrice")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(o => o.DiscountAmount)
            .HasColumnName("DiscountAmount")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(o => o.ShippingAmount)
            .HasColumnName("ShippingAmount")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(o => o.Status)
            .HasColumnName("Status")
            .HasConversion<string>()
            .IsRequired();

        builder.OwnsOne(o => o.CustomerInfo, ci =>
        {
            ci.Property(c => c.Name)
                .HasColumnName("CustomerName")
                .HasMaxLength(255)
                .IsRequired();

            ci.Property(c => c.Email)
                .HasColumnName("CustomerEmail")
                .HasMaxLength(255)
                .IsRequired();

            ci.Property(c => c.PhoneNumber)
                .HasColumnName("CustomerPhone")
                .HasMaxLength(20);
        });

        builder.OwnsOne(o => o.ShippingAddress, sa =>
        {

            sa.Property(a => a.City)
                .HasColumnName("ShippingCity")
                .HasMaxLength(255)
                .IsRequired();

            sa.Property(a => a.Longitude)
                .HasColumnName("ShippingState")
                .HasMaxLength(100);

            sa.Property(a => a.Latitude)
                .HasColumnName("ShippingZipCode")
                .HasMaxLength(20)
                .IsRequired();

            sa.Property(a => a.FullAddress)
                .HasColumnName("ShippingCountry")
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.Property(x => x.PaymentMethod)
            .HasColumnName("PaymentMethod")
            .HasConversion<string>()
            .IsRequired();

        builder.HasMany(o => o.OrderItems)
            .WithOne()
            .HasForeignKey("OrderId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(o => o.OrderDate).HasColumnName("OrderDate");

 
        builder.HasIndex(o => o.TotalPrice);
    }
}
