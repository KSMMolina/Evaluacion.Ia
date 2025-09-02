using Api_test_ia.Application.Abstractions.Persistence.Users;
using Api_test_ia.Application.Common;
using Api_test_ia.Application.Dtos;
using MediatR;

namespace Api_test_ia.Application.UseCases.Users.Queries.Handler
{
    internal sealed class ListUsersHandler(IUserQueries q) : IRequestHandler<ListUsersQuery, PagedResult<UserDto>>
    {
        public Task<PagedResult<UserDto>> Handle(ListUsersQuery r, CancellationToken ct)
            => q.ListAsync(r.Search, r.Role, r.Sort, r.Dir, r.Page, r.PageSize, ct);
    }
}
