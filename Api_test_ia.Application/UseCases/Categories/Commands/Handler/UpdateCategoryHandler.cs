using Api_test_ia.Application.Abstractions.Persistence;
using Api_test_ia.Application.Abstractions.Persistence.Categories;
using MediatR;

namespace Api_test_ia.Application.UseCases.Categories.Commands.Handler
{

    internal sealed class UpdateCategoryHandler(ICategoryCommands repo, IUnitOfWork uow) : IRequestHandler<UpdateCategoryCommand>
    {
        public async Task Handle(UpdateCategoryCommand r, CancellationToken ct)
        {
            var c = await repo.GetByIdAsync(r.Id, ct) ?? throw new KeyNotFoundException();
            c.Update(r.Name, r.ParentCategoryId, r.IsActive);
            await uow.SaveChangesAsync(ct);
        }
    }
}
