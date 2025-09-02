using Api_test_ia.Application.Abstractions.Persistence;
using Api_test_ia.Application.Abstractions.Persistence.Products;
using MediatR;

namespace Api_test_ia.Application.UseCases.Products.Commands.Handler
{
    internal sealed class ToggleProductHandler(IProductCommands repo, IUnitOfWork uow) : IRequestHandler<ToggleProductCommand>
    {
        public async Task Handle(ToggleProductCommand r, CancellationToken ct)
        {
            var p = await repo.GetByIdAsync(r.Id, false, ct) ?? throw new KeyNotFoundException();
            p.Toggle();
            await uow.SaveChangesAsync(ct);
        }
    }
}