using Api_test_ia.Application.Abstractions.Persistence.Categories;
using Api_test_ia.Application.Dtos;
using MediatR;

namespace Api_test_ia.Application.UseCases.Categories.Queries.Handler
{
    internal sealed class GetPublicCategoriesTreeHandler(ICategoryQueries q)
    : IRequestHandler<GetPublicCategoriesTreeQuery, List<CategoryNodeDto>>
    {
        public Task<List<CategoryNodeDto>> Handle(GetPublicCategoriesTreeQuery r, CancellationToken ct)
            => q.GetTreeAsync(r.Flat, true, ct);
    }
}
