using Api_test_ia.Application.Abstractions.Persistence;
using Api_test_ia.Application.Abstractions.Persistence.Products;
using MediatR;

namespace Api_test_ia.Application.UseCases.Products.Commands.Handler
{
    internal sealed class UpdateProductHandler(IProductCommands repo, IUnitOfWork uow) : IRequestHandler<UpdateProductCommand>
    {
        public async Task Handle(UpdateProductCommand r, CancellationToken ct)
        {
            try
            {
                var p = await repo.GetByIdAsync(r.Id, includeImages: false, ct) ?? throw new KeyNotFoundException();
                p.Update(r.Name, r.Description, r.Price, r.CategoryId, r.IsActive);
                await uow.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                throw new InvalidOperationException("El SKU ya existe.");
            }
        }
    }
}