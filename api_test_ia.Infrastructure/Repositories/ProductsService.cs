using Api_test_ia.Application.Abstractions;
using Api_test_ia.Application.Common;
using Api_test_ia.Application.Dtos;
using Api_test_ia.Domain.Entities;
using Api_test_ia.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Api_test_ia.Infrastructure.Repositories
{
    public sealed class ProductsService(AppDbContext db) : IProductsService
    {
        public async Task<PagedResult<ProductListItemDto>> ListAsync(string? search, int? categoryId, bool? isActive, string? sort, string? dir, int page, int pageSize, CancellationToken ct)
        {
            var q = db.Products.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(p => p.Name.Contains(search) || p.Sku.Contains(search));
            if (categoryId.HasValue) q = q.Where(p => p.CategoryId == categoryId);
            if (isActive.HasValue) q = q.Where(p => p.IsActive == isActive);

            // sort
            var asc = string.Equals(dir, "asc", StringComparison.OrdinalIgnoreCase);
            q = (sort?.ToLowerInvariant()) switch
            {
                "price" => asc ? q.OrderBy(p => p.Price) : q.OrderByDescending(p => p.Price),
                "createdat" => asc ? q.OrderBy(p => p.CreatedAt) : q.OrderByDescending(p => p.CreatedAt),
                "name" => asc ? q.OrderBy(p => p.Name) : q.OrderByDescending(p => p.Name),
                _ => q.OrderBy(p => p.Id)
            };

            var mapped = q.Select(p => new ProductListItemDto(p.Id, p.Sku, p.Name, p.Price, p.CategoryId, p.IsActive, p.CreatedAt));
            return await mapped.ToPagedAsync(page, pageSize, ct);
        }

        public async Task<ProductDetailDto?> GetAsync(int id, CancellationToken ct)
        {
            var p = await db.Products.AsNoTracking()
                .Include(x => x.Images.OrderBy(i => i.SortOrder))
                .FirstOrDefaultAsync(x => x.Id == id, ct);
            return p is null ? null :
                new ProductDetailDto(p.Id, p.Sku, p.Name, p.Description, p.Price, p.IsActive, p.CategoryId, p.CreatedAt,
                    p.Images.Select(i => new ProductImageDto(i.Id, i.Url, i.AltText, i.SortOrder)).ToList());
        }

        public async Task<int> CreateAsync(string sku, string name, decimal price, string? description, int? categoryId, bool? isActive, CancellationToken ct)
        {
            if (await db.Products.AnyAsync(x => x.Sku == sku, ct)) throw new InvalidOperationException("SKU ya existe");
            var entity = new Product(sku, name, price, description, categoryId);
            if (isActive.HasValue && !isActive.Value) entity.Toggle(); // por defecto true
            db.Products.Add(entity);
            await db.SaveChangesAsync(ct);
            return entity.Id;
        }

        public async Task UpdateAsync(int id, string? name, string? description, decimal? price, int? categoryId, bool? isActive, CancellationToken ct)
        {
            var p = await db.Products.FindAsync(new object?[] { id }, ct) ?? throw new KeyNotFoundException();
            p.Update(name, description, price, categoryId, isActive);
            await db.SaveChangesAsync(ct);
        }

        public async Task ToggleAsync(int id, CancellationToken ct)
        {
            var p = await db.Products.FindAsync(new object?[] { id }, ct) ?? throw new KeyNotFoundException();
            p.Toggle();
            await db.SaveChangesAsync(ct);
        }

        public async Task<int> AddImageAsync(int id, string url, string? altText, int? sortOrder, CancellationToken ct)
        {
            var exists = await db.Products.AnyAsync(x => x.Id == id, ct);
            if (!exists) throw new KeyNotFoundException();
            var img = new ProductImage(id, url, altText, sortOrder ?? 0);
            db.ProductImages.Add(img);
            await db.SaveChangesAsync(ct);
            return img.Id;
        }

        public async Task EditImageAsync(int id, int imgId, string? altText, int? sortOrder, CancellationToken ct)
        {
            var img = await db.ProductImages.FirstOrDefaultAsync(i => i.Id == imgId && i.ProductId == id, ct) ?? throw new KeyNotFoundException();
            img.Edit(altText, sortOrder);
            await db.SaveChangesAsync(ct);
        }

        public async Task DeleteImageAsync(int id, int imgId, CancellationToken ct)
        {
            var img = await db.ProductImages.FirstOrDefaultAsync(i => i.Id == imgId && i.ProductId == id, ct) ?? throw new KeyNotFoundException();
            db.ProductImages.Remove(img);
            await db.SaveChangesAsync(ct);
        }
    }
}
