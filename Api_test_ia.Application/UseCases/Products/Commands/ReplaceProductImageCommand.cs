using MediatR;

namespace Api_test_ia.Application.UseCases.Products.Commands
{
    public sealed record ReplaceProductImageCommand(
        int ProductId, int ImageId,
        string FileName, string ContentType, Stream Content
    ) : IRequest<string>;
}
