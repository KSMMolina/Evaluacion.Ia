using Api_test_ia.Application.Abstractions.Persistence;
using Api_test_ia.Application.Abstractions.Persistence.Products;
using MediatR;

namespace Api_test_ia.Application.UseCases.Products.Commands.Handler
{
    internal sealed class EditProductImageHandler(IProductCommands repo, IUnitOfWork uow) : IRequestHandler<EditProductImageCommand>
    {
        public async Task Handle(EditProductImageCommand r, CancellationToken ct)
        {
            var p = await repo.GetByIdAsync(r.ProductId, includeImages: true, ct) ?? throw new KeyNotFoundException();
            var img = p.Images.FirstOrDefault(i => i.Id == r.ImageId) ?? throw new KeyNotFoundException();
            img.Edit(r.AltText, r.SortOrder);
            await uow.SaveChangesAsync(ct);
        }
    }
}
