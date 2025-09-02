using MediatR;

namespace Api_test_ia.Application.UseCases.Products.Commands
{
    public sealed record AddProductImageCommand(int ProductId, string Url, string? AltText, int? SortOrder) : IRequest<int>;
}
