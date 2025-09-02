using Api_test_ia.Application.Abstractions.Persistence.Products;
using Api_test_ia.Application.Common;
using Api_test_ia.Application.Dtos;
using Api_test_ia.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Api_test_ia.Infrastructure.Repositories.Products
{
    public sealed class EfProductQueries(AppDbContext db) : IProductQueries
    {
        public async Task<PagedResult<ProductListItemDto>> ListAsync(string? search, int? categoryId, bool onlyActive, string? sort, string? dir, int page, int pageSize, decimal? minPrice, decimal? maxPrice, CancellationToken ct)
        {
            var q = db.Products.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(p => p.Name.Contains(search) || p.Sku.Contains(search));

            if (onlyActive) q = q.Where(p => p.IsActive);
            if (categoryId.HasValue) q = q.Where(p => p.CategoryId == categoryId);
            if (minPrice.HasValue) q = q.Where(p => p.Price >= minPrice.Value);
            if (maxPrice.HasValue) q = q.Where(p => p.Price <= maxPrice.Value);

            var asc = string.Equals(dir, "asc", StringComparison.OrdinalIgnoreCase);
            q = (sort?.ToLowerInvariant()) switch
            {
                "name" => asc ? q.OrderBy(p => p.Name) : q.OrderByDescending(p => p.Name),
                "price" => asc ? q.OrderBy(p => p.Price) : q.OrderByDescending(p => p.Price),
                "createdat" => asc ? q.OrderBy(p => p.CreatedAt) : q.OrderByDescending(p => p.CreatedAt),
                _ => asc ? q.OrderBy(p => p.Id) : q.OrderByDescending(p => p.Id)
            };

            var total = await q.LongCountAsync(ct);
            var items = await q.Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .Select(p => new ProductListItemDto(
                                   p.Id, p.Sku, p.Name, p.Price, p.CategoryId, p.IsActive, p.CreatedAt))
                               .ToListAsync(ct);

            return new PagedResult<ProductListItemDto>(items, page, pageSize, total);
        }

        public async Task<ProductDetailDto?> GetDetailAsync(int id, CancellationToken ct)
        {
            var p = await db.Products.AsNoTracking()
                .Include(x => x.Images.OrderBy(i => i.SortOrder))
                .FirstOrDefaultAsync(x => x.Id == id, ct);

            return p is null ? null :
                new ProductDetailDto(p.Id, p.Sku, p.Name, p.Description, p.Price, p.IsActive, p.CategoryId, p.CreatedAt,
                    p.Images.Select(i => new ProductImageDto(i.Id, i.Url, i.AltText, i.SortOrder)).ToList());
        }

        public async Task<ProductImageDto?> GetImageAsync(int productId, int imageId, CancellationToken ct)
        {
            var img = await db.ProductImages.AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == imageId && i.ProductId == productId, ct);
            return img is null ? null : new ProductImageDto(img.Id, img.Url, img.AltText, img.SortOrder);
        }
    }
}
