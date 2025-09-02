using Api_test_ia.Application.Abstractions.Persistence;
using Api_test_ia.Application.Abstractions.Persistence.Products;
using Api_test_ia.Domain.Entities;
using MediatR;

namespace Api_test_ia.Application.UseCases.Products.Commands.Handler
{
    internal sealed class CreateProductHandler(IProductCommands repo, IUnitOfWork uow) : IRequestHandler<CreateProductCommand, int>
    {
        public async Task<int> Handle(CreateProductCommand r, CancellationToken ct)
        {
            try
            {
                var p = new Product(r.Sku, r.Name, r.Price, r.Description, r.CategoryId);
                if (r.IsActive.HasValue && !r.IsActive.Value) p.Toggle();
                await repo.AddAsync(p, ct);
                await uow.SaveChangesAsync(ct);
                return p.Id;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                throw new InvalidOperationException("El SKU ya existe.");
            }
        }
    }
}
