using Api_test_ia.Application.Abstractions.Persistence;
using Api_test_ia.Application.Abstractions.Persistence.Categories;
using MediatR;

namespace Api_test_ia.Application.UseCases.Categories.Commands.Handler
{
    internal sealed class ToggleCategoryHandler(ICategoryCommands repo, IUnitOfWork uow) : IRequestHandler<ToggleCategoryCommand>
    {
        public async Task Handle(ToggleCategoryCommand r, CancellationToken ct)
        {
            var c = await repo.GetByIdAsync(r.Id, ct) ?? throw new KeyNotFoundException();
            c.Toggle();
            await uow.SaveChangesAsync(ct);
        }
    }
}
