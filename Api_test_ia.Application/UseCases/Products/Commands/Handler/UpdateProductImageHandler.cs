using Api_test_ia.Application.Abstractions.Persistence;
using Api_test_ia.Application.Abstractions.Persistence.Products;
using Api_test_ia.Application.Abstractions.Storage;
using Api_test_ia.Domain.Entities;
using MediatR;

namespace Api_test_ia.Application.UseCases.Products.Commands.Handler
{
    internal sealed class UploadProductImagesHandler(
        IProductCommands repo,
        IUnitOfWork uow,
        IImageStorage storage
    ) : IRequestHandler<UploadProductImagesCommand, List<(int ImageId, string Url)>>
    {
        private static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase)
    { "image/jpeg", "image/png" };

        private const long MaxSizeBytes = 2_000_000; // 2MB

        public async Task<List<(int ImageId, string Url)>> Handle(UploadProductImagesCommand r, CancellationToken ct)
        {
            if (r.Files is null || r.Files.Count == 0)
                throw new FluentValidation.ValidationException("Debe enviar al menos un archivo.", []);

            var created = new List<ProductImage>();

            foreach (var f in r.Files)
            {
                if (f.Length <= 0) continue;
                if (f.Length > MaxSizeBytes)
                    throw new FluentValidation.ValidationException($"El archivo {f.FileName} supera 2MB.", []);
                if (!Allowed.Contains(f.ContentType))
                    throw new FluentValidation.ValidationException($"El archivo {f.FileName} no es jpg/png.", []);

                var ext = Path.GetExtension(f.FileName).ToLowerInvariant();
                var safeName = $"{Guid.NewGuid():N}{ext}";
                var container = $"products/{r.ProductId}";

                // guarda físico
                var url = await storage.SaveAsync(safeName, f.Content, f.ContentType, container, ct);

                // registra en BD
                var entity = new ProductImage(r.ProductId, url, altText: null, sortOrder: 0);
                await repo.AddImageAsync(entity, ct);
                created.Add(entity);
            }

            await uow.SaveChangesAsync(ct);
            return created.Select(x => (x.Id, x.Url)).ToList();
        }
    }
}
