using Api_test_ia.Application.Common;
using MediatR;

namespace Api_test_ia.Application.UseCases.Products.Commands
{
    public sealed record UploadProductImagesCommand(
        int ProductId,
        List<IncomingFile> Files
    ) : IRequest<List<(int ImageId, string Url)>>;
}
