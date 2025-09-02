using MediatR;

namespace Api_test_ia.Application.UseCases.Users.Commands
{
    public sealed record DeleteUserCommand(int Id) : IRequest;
}
