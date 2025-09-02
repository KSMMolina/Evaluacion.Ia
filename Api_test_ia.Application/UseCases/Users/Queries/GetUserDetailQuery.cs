using Api_test_ia.Application.Dtos;
using MediatR;

namespace Api_test_ia.Application.UseCases.Users.Queries
{
    public sealed record GetUserDetailQuery(int Id) : IRequest<UserDetailDto?>;
}
