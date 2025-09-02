using Api_test_ia.Application.Dtos;
using MediatR;

namespace Api_test_ia.Application.UseCases.Auth.Commands
{
    public sealed record LoginCommand(string Email, string Password) : IRequest<(AuthTokens tokens, string refreshToken)>;
}
