using Api_test_ia.Application.Abstractions.Persistence.Users;
using Api_test_ia.Application.Dtos;
using MediatR;

namespace Api_test_ia.Application.UseCases.Users.Queries.Handler
{
    internal sealed class GetUserDetailHandler(IUserQueries q) : IRequestHandler<GetUserDetailQuery, UserDetailDto?>
    {
        public Task<UserDetailDto?> Handle(GetUserDetailQuery r, CancellationToken ct) => q.GetAsync(r.Id, ct);
    }
}
