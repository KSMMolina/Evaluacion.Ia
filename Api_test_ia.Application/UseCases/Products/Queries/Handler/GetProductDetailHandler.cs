using Api_test_ia.Application.Abstractions.Persistence.Products;
using Api_test_ia.Application.Dtos;
using MediatR;

namespace Api_test_ia.Application.UseCases.Products.Queries.Handler
{
    internal sealed class GetProductDetailHandler(IProductQueries q) : IRequestHandler<GetProductDetailQuery, ProductDetailDto?>
    {
        public Task<ProductDetailDto?> Handle(GetProductDetailQuery r, CancellationToken ct) => q.GetDetailAsync(r.Id, ct);
    }
}
