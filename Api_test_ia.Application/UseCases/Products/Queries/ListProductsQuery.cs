using Api_test_ia.Application.Common;
using Api_test_ia.Application.Dtos;
using MediatR;

namespace Api_test_ia.Application.UseCases.Products.Queries
{
    public sealed record ListProductsQuery(
        string? Search, int? CategoryId, bool OnlyActive,
        string? Sort, string? Dir, int Page, int PageSize,
        decimal? MinPrice, decimal? MaxPrice
    ) : IRequest<PagedResult<ProductListItemDto>>;
}
