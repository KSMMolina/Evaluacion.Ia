using Api_test_ia.Application.Dtos;
using MediatR;

namespace Api_test_ia.Application.UseCases.Auth.Commands
{
    public sealed record RefreshTokenCommand(string RefreshToken)
    : IRequest<(AuthTokens tokens, string refreshToken)>;
}
