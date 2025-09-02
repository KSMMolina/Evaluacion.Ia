using MediatR;

namespace Api_test_ia.Application.UseCases.Auth.Commands
{
    public sealed record LogoutCommand(string RefreshToken) : IRequest;
}
