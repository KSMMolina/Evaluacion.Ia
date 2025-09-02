using Api_test_ia.Domain.Entities;

namespace Api_test_ia.Application.Abstractions.Persistence.Products
{
    public interface IProductCommands
    {
        Task<bool> SkuExistsAsync(string sku, CancellationToken ct);
        Task<Product?> GetByIdAsync(int id, bool includeImages, CancellationToken ct);
        Task AddAsync(Product entity, CancellationToken ct);
        void RemoveImage(ProductImage image);
        Task AddImageAsync(ProductImage image, CancellationToken ct);
    }
}
