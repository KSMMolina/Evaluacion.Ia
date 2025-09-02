using Api_test_ia.Application.Abstractions.Persistence;
using Api_test_ia.Application.Abstractions.Persistence.Products;
using Api_test_ia.Application.Abstractions.Storage;
using MediatR;

namespace Api_test_ia.Application.UseCases.Products.Commands.Handler
{
    internal sealed class ReplaceProductImageHandler(
        IProductCommands repo, IUnitOfWork uow, IImageStorage storage
    ) : IRequestHandler<ReplaceProductImageCommand, string>
    {
        public async Task<string> Handle(ReplaceProductImageCommand r, CancellationToken ct)
        {
            var product = await repo.GetByIdAsync(r.ProductId, includeImages: true, ct)
                          ?? throw new KeyNotFoundException("Producto no encontrado");
            var img = product.Images.FirstOrDefault(i => i.Id == r.ImageId)
                      ?? throw new KeyNotFoundException("Imagen no encontrada");

            // 1) sube nuevo archivo
            var container = $"products/{r.ProductId}";
            var ext = Path.GetExtension(r.FileName);
            var safeName = $"{Guid.NewGuid():N}{ext}";
            var newUrl = await storage.SaveAsync(safeName, r.Content, r.ContentType, container, ct);

            // 2) cambia URL en BD
            var oldUrl = img.Url;
            img.ReplaceUrl(newUrl);
            await uow.SaveChangesAsync(ct);

            // 3) borra archivo anterior (si existía)
            if (!string.IsNullOrWhiteSpace(oldUrl))
                await storage.DeleteAsync(oldUrl, ct);

            return newUrl;
        }
    }
}
