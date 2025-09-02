using Api_test_ia.Application.Abstractions.Persistence.Categories;
using Api_test_ia.Application.Dtos;
using MediatR;

namespace Api_test_ia.Application.UseCases.Categories.Queries.Handler
{
    internal sealed class GetAdminCategoriesTreeHandler(ICategoryQueries q)
    : IRequestHandler<GetAdminCategoriesTreeQuery, List<CategoryNodeDto>>
    {
        public Task<List<CategoryNodeDto>> Handle(GetAdminCategoriesTreeQuery r, CancellationToken ct)
            => q.GetTreeAsync(r.Flat, false, ct);
    }
}
