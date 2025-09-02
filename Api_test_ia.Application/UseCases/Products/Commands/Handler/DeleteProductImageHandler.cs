using Api_test_ia.Application.Abstractions.Persistence;
using Api_test_ia.Application.Abstractions.Persistence.Products;
using Api_test_ia.Application.Abstractions.Storage;
using MediatR;

namespace Api_test_ia.Application.UseCases.Products.Commands.Handler
{
    internal sealed class DeleteProductImageHandler(
        IProductCommands repo,
        IUnitOfWork uow,
        IImageStorage storage
    ) : IRequestHandler<DeleteProductImageCommand>
    {
        public async Task Handle(DeleteProductImageCommand r, CancellationToken ct)
        {
            // Trae el producto con imágenes (tracked)
            var product = await repo.GetByIdAsync(r.ProductId, includeImages: true, ct)
                          ?? throw new KeyNotFoundException("Producto no encontrado");

            // Busca la imagen dentro del agregado
            var image = product.Images.FirstOrDefault(i => i.Id == r.ImageId)
                        ?? throw new KeyNotFoundException("Imagen no encontrada");

            // 1) quita del agregado (y del DbContext) usando TU puerto existente
            repo.RemoveImage(image);

            // 2) persiste primero para no borrar archivo si la operación falla
            await uow.SaveChangesAsync(ct);

            // 3) elimina archivo físico (ignora si no existe)
            await storage.DeleteAsync(image.Url, ct);
        }
    }
}
