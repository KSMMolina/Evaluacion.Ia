using Api_test_ia.Application.Abstractions.Persistence.Products;
using Api_test_ia.Application.Common;
using Api_test_ia.Application.Dtos;
using MediatR;

namespace Api_test_ia.Application.UseCases.Products.Queries.Handler
{
    internal sealed class ListProductsHandler(IProductQueries q)
        : IRequestHandler<ListProductsQuery, PagedResult<ProductListItemDto>>
    {
        public Task<PagedResult<ProductListItemDto>> Handle(ListProductsQuery r, CancellationToken ct)
            => q.ListAsync(r.Search, r.CategoryId, r.OnlyActive, r.Sort, r.Dir,
                           r.Page, r.PageSize, r.MinPrice, r.MaxPrice, ct);
    }
}
