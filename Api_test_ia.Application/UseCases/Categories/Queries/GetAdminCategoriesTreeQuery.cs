using Api_test_ia.Application.Dtos;
using MediatR;

namespace Api_test_ia.Application.UseCases.Categories.Queries
{
    public sealed record GetAdminCategoriesTreeQuery(bool Flat) : IRequest<List<CategoryNodeDto>>;
}
