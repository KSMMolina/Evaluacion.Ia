//using Api_test_ia.Application.Dtos;
//using Api_test_ia.Application.Repository;
//using MediatR;

//namespace Api_test_ia.Application.UseCases.Categories.Queries.Handler;

//public class ListCategoriesHandler : IRequestHandler<ListCategoriesQuery, IReadOnlyList<CategoryListItemDto>>
//{
//    private readonly ICategoryRepository _repo;

//    public ListCategoriesHandler(ICategoryRepository repo)
//    {
//        _repo = repo;
//    }

//    public async Task<IReadOnlyList<CategoryListItemDto>> Handle(ListCategoriesQuery request, CancellationToken cancellationToken)
//    {
//        var categories = await _repo.ListAsync(request.OnlyActive, cancellationToken);
//        return categories.Select(c => new CategoryListItemDto(
//            c.Id,
//            c.Name,
//            c.ParentCategoryId,
//            c.IsActive
//        )).ToList();
//    }
//}