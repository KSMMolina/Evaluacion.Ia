using MediatR;

namespace Api_test_ia.Application.UseCases.Categories.Commands
{
    public sealed record CreateCategoryCommand(string Name, int? ParentCategoryId, bool? IsActive) : IRequest<int>;
}
