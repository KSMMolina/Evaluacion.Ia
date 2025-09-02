using MediatR;

namespace Api_test_ia.Application.UseCases.Users.Commands
{
    public sealed record CreateUserCommand(string Email, string Password, string Role) : IRequest<int>;
}
