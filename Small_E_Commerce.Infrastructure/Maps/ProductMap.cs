using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Small_E_Commerce.Products;

namespace Small_E_Commerce.Infrastructure.Maps;

public class ProductMap : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("Name")
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(x => x.SKU)
            .HasColumnName("SkU")
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Description)
            .HasColumnName("Description")
            .HasMaxLength(1000);

        builder.Property(x => x.Price)
            .HasColumnName("Price")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.Stock)
            .HasColumnName("Stock")
            .IsRequired();

        builder.Property(x => x.LowStockThreshold)
            .HasColumnName("LowStockThreshold")
            .IsRequired();

        builder.Property(x => x.ExpirationDate)
            .HasColumnName("ExpirationDate");

        builder.Property(x => x.Category)
            .HasColumnName("Category")
            .HasMaxLength(255);

        builder.OwnsOne(x => x.ImageUrl, options =>
        {
            options.Property(p => p.Value)
                .HasColumnName("ImageUrl")
                .HasMaxLength(255);
        });

        builder.OwnsOne(x => x.ProductOptions, options =>
        {
            options.Property(p => p.Name)
                .HasColumnName("OptionName")
                .HasMaxLength(255);
            
            options.Property(p => p.Value)
                .HasColumnName("OptionValue")
                .HasMaxLength(255);
        });

        builder.Property(x => x.Status)
            .HasColumnName("Status")
            .HasConversion<string>() 
            .IsRequired();
        
        builder.HasIndex(x => x.SKU).IsUnique(); 
    }
}
