using MediatR;

namespace Api_test_ia.Application.UseCases.Products.Commands
{
    public sealed record UpdateProductCommand(int Id, string? Name, string? Description, decimal? Price, int? CategoryId, bool? IsActive) : IRequest;
}
