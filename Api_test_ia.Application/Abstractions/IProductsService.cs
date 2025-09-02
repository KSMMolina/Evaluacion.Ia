using Api_test_ia.Application.Common;
using Api_test_ia.Application.Dtos;

namespace Api_test_ia.Application.Abstractions
{
    public interface IProductsService
    {
        Task<PagedResult<ProductListItemDto>> ListAsync(string? search, int? categoryId, bool? isActive, string? sort, string? dir, int page, int pageSize, CancellationToken ct);
        Task<ProductDetailDto?> GetAsync(int id, CancellationToken ct);
        Task<int> CreateAsync(string sku, string name, decimal price, string? description, int? categoryId, bool? isActive, CancellationToken ct);
        Task UpdateAsync(int id, string? name, string? description, decimal? price, int? categoryId, bool? isActive, CancellationToken ct);
        Task ToggleAsync(int id, CancellationToken ct);
        Task<int> AddImageAsync(int id, string url, string? altText, int? sortOrder, CancellationToken ct);
        Task EditImageAsync(int id, int imgId, string? altText, int? sortOrder, CancellationToken ct);
        Task DeleteImageAsync(int id, int imgId, CancellationToken ct);
    }
}
