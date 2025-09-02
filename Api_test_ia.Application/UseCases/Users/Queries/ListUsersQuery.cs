using Api_test_ia.Application.Common;
using Api_test_ia.Application.Dtos;
using MediatR;

namespace Api_test_ia.Application.UseCases.Users.Queries
{
    public sealed record ListUsersQuery(string? Search, string? Role, string? Sort, string? Dir, int Page = 1, int PageSize = 12)
    : IRequest<PagedResult<UserDto>>;
}
