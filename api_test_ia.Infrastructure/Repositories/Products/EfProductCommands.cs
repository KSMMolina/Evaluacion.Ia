using Api_test_ia.Application.Abstractions.Persistence.Products;
using Api_test_ia.Domain.Entities;
using Api_test_ia.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Api_test_ia.Infrastructure.Repositories.Products
{
    public sealed class EfProductCommands(AppDbContext db) : IProductCommands
    {
        public Task<bool> SkuExistsAsync(string sku, CancellationToken ct)
            => db.Products.AnyAsync(p => p.Sku == sku, ct);

        public Task<Product?> GetByIdAsync(int id, bool includeImages, CancellationToken ct)
            => includeImages
                ? db.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id, ct)
                : db.Products.FirstOrDefaultAsync(p => p.Id == id, ct);

        public Task AddAsync(Product entity, CancellationToken ct)
        {
            db.Products.Add(entity);
            return Task.CompletedTask;
        }

        public void RemoveImage(ProductImage image)
        {
            // Elimina la entidad del contexto (funciona aunque no tengas la FK cargada)
            db.ProductImages.Remove(image);
        }

        public Task AddImageAsync(ProductImage image, CancellationToken ct)
        {
            db.ProductImages.Add(image);
            return Task.CompletedTask;
        }
    }
}
