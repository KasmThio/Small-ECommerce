using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Small_E_Commerce.Identity.Users;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(u => u.Email).IsUnique(false);
        builder.Property(x => x.IsAdmin).HasColumnName("IsAdmin");
        builder.HasIndex(u => u.NormalizedUserName).IsUnique(false);
        builder.Property(x => x.CreatedAt).HasColumnName("CreatedAt");
    }
}