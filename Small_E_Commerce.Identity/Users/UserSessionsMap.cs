using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Small_E_Commerce.Identity.Users;

public sealed class UserSessionsMap : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.ToTable("UserSessionStore", "identity");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SessionId).HasColumnName("SessionId").IsRequired();
        builder.HasIndex(x => x.SessionId).IsUnique();
        builder.Property(x => x.UserId).HasColumnName("UserId");
        builder.Property(x => x.CreatedAt).HasColumnName("CreatedAt");
        builder.Property(x => x.UpdatedAt).HasColumnName("UpdatedAt");

        builder.HasOne<User>().WithMany().HasForeignKey(x => x.UserId);
    }
}