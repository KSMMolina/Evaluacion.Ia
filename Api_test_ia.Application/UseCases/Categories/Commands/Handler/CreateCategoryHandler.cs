using Api_test_ia.Application.Abstractions.Persistence;
using Api_test_ia.Application.Abstractions.Persistence.Categories;
using Api_test_ia.Domain.Entities;
using MediatR;

namespace Api_test_ia.Application.UseCases.Categories.Commands.Handler
{
    internal sealed class CreateCategoryHandler(ICategoryCommands repo, IUnitOfWork uow) : IRequestHandler<CreateCategoryCommand, int>
    {
        public async Task<int> Handle(CreateCategoryCommand r, CancellationToken ct)
        {
            var c = new Category(r.Name, r.ParentCategoryId, r.IsActive ?? true);
            await repo.AddAsync(c, ct);
            await uow.SaveChangesAsync(ct);
            return c.Id;
        }
    }
}
