using Api_test_ia.Application.Common;
using Api_test_ia.Application.Dtos;

namespace Api_test_ia.Application.Abstractions.Persistence.Products
{
    public interface IProductQueries
    {
        //To do: remove the old method after testing the new one
        //Task<PagedResult<ProductListItemDto>> ListAsync(string? search, int? categoryId, bool? isActive, string? sort, string? dir, int page, int pageSize, CancellationToken ct);

        Task<PagedResult<ProductListItemDto>> ListAsync(
            string? search, int? categoryId, bool onlyActive,
            string? sort, string? dir, int page, int pageSize,
            decimal? minPrice, decimal? maxPrice, CancellationToken ct);

        Task<ProductDetailDto?> GetDetailAsync(int id, CancellationToken ct);

        Task<ProductImageDto?> GetImageAsync(int productId, int imageId, CancellationToken ct);
    }
}
