using Api_test_ia.Application.Dtos;
using MediatR;

namespace Api_test_ia.Application.UseCases.Products.Queries
{
    public sealed record GetProductDetailQuery(int Id) : IRequest<ProductDetailDto?>;
}
