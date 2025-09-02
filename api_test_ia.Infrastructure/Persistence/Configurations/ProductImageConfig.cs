using Api_test_ia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api_test_ia.Infrastructure.Persistence.Configurations
{
    public sealed class ProductImageConfig : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> e)
        {
            e.ToTable("ProductImage", "dbo");
            e.HasKey(x => x.Id);
            e.Property(x => x.Url).HasMaxLength(400).IsRequired();
            e.Property(x => x.AltText).HasMaxLength(150);
            e.Property(x => x.SortOrder).HasDefaultValue(0);
            e.HasOne(x => x.Product).WithMany(x => x.Images)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(x => new { x.ProductId, x.SortOrder }).HasDatabaseName("IX_ProductImage_Product_Sort");
        }
    }
}