using MediatR;

namespace Api_test_ia.Application.UseCases.Products.Commands
{
    public sealed record DeleteProductImageCommand(int ProductId, int ImageId) : IRequest;
}
