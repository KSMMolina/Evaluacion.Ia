using Api_test_ia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api_test_ia.Infrastructure.Persistence.Configurations
{
    public sealed class UserRefreshTokenConfig : IEntityTypeConfiguration<UserRefreshToken>
    {
        public void Configure(EntityTypeBuilder<UserRefreshToken> e)
        {
            e.ToTable("UserRefreshToken", "dbo");
            e.HasKey(x => x.Id);
            e.Property(x => x.TokenHash).HasMaxLength(256).IsRequired();
            e.Property(x => x.ExpiresAt).HasColumnType("datetime2");
            e.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
