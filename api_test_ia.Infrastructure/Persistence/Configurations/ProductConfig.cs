using Api_test_ia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api_test_ia.Infrastructure.Persistence.Configurations
{
    public sealed class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> e)
        {
            e.ToTable("Product", "dbo");
            e.HasKey(x => x.Id);
            e.Property(x => x.Sku).HasMaxLength(50).IsRequired();
            e.HasIndex(x => x.Sku).IsUnique();
            e.Property(x => x.Name).HasMaxLength(200).IsRequired();
            e.Property(x => x.Description).HasMaxLength(1000);
            e.Property(x => x.Price).HasColumnType("decimal(18,2)").IsRequired();
            e.ToTable(t => t.HasCheckConstraint("CK_Product_Price", "[Price] >= 0"));
            e.Property(x => x.IsActive).HasDefaultValue(true);
            e.Property(x => x.CreatedAt).HasColumnType("datetime2").HasDefaultValueSql("SYSUTCDATETIME()");
            e.HasOne(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryId);
            e.HasIndex(x => x.Name).HasDatabaseName("IX_Product_Name");
            e.HasIndex(x => new { x.CategoryId }).HasDatabaseName("IX_Product_Category");
            e.HasIndex(x => new { x.IsActive, x.CreatedAt }).HasDatabaseName("IX_Product_IsActive_CreatedAt");
        }
    }
}