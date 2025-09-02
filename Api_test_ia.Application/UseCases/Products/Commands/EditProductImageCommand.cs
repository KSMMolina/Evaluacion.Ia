using MediatR;

namespace Api_test_ia.Application.UseCases.Products.Commands
{
    public sealed record EditProductImageCommand(int ProductId, int ImageId, string? AltText, int? SortOrder) : IRequest;
}
