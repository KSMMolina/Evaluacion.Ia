using MediatR;

namespace Api_test_ia.Application.UseCases.Products.Commands
{
    public sealed record CreateProductCommand(string Sku, string Name, decimal Price, string? Description, int? CategoryId, bool? IsActive) : IRequest<int>;
}
