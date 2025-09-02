using Api_test_ia.Application.Abstractions.Persistence;
using Api_test_ia.Application.Abstractions.Persistence.Products;
using Api_test_ia.Domain.Entities;
using MediatR;

namespace Api_test_ia.Application.UseCases.Products.Commands.Handler
{
    internal sealed class AddProductImageHandler(IProductCommands repo, IUnitOfWork uow) : IRequestHandler<AddProductImageCommand, int>
    {
        public async Task<int> Handle(AddProductImageCommand r, CancellationToken ct)
        {
            var p = await repo.GetByIdAsync(r.ProductId, includeImages: false, ct) ?? throw new KeyNotFoundException();
            var img = new ProductImage(p.Id, r.Url, r.AltText, r.SortOrder ?? 0);
            await repo.AddImageAsync(img, ct);
            await uow.SaveChangesAsync(ct);
            return img.Id;
        }
    }
}
