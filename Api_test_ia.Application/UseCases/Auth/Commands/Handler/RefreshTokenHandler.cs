using Api_test_ia.Application.Abstractions.Persistence.Auth;
using Api_test_ia.Application.Abstractions.Security;
using Api_test_ia.Application.Dtos;
using MediatR;

namespace Api_test_ia.Application.UseCases.Auth.Commands.Handler
{
    internal sealed class RefreshTokenHandler(
    IRefreshTokenStore store,
    IUserRepository users,
    IPasswordHasher hasher,
    IJwtProvider jwt)
    : IRequestHandler<RefreshTokenCommand, (AuthTokens tokens, string refreshToken)>
    {
        public async Task<(AuthTokens tokens, string refreshToken)> Handle(RefreshTokenCommand r, CancellationToken ct)
        {
            // Buscar el refresh token válido y que coincida con el valor enviado
            var stored = await store.FindValidAsync(r.RefreshToken, ct)
                ?? throw new UnauthorizedAccessException();

            // Cargar usuario
            var user = await users.FindByIdAsync(stored.UserId, ct)
                ?? throw new UnauthorizedAccessException();

            // Rotar: revoco el anterior y creo uno nuevo
            await store.RevokeAsync(stored, ct);

            var accessToken = jwt.CreateAccessToken(user);
            var (newRefresh, exp) = jwt.CreateRefreshToken();

            // Guardo el nuevo refresh (hashed)
            await store.AddAsync(user.Id, hasher.Hash(newRefresh), exp, ct);

            return (new AuthTokens(accessToken, 15 * 60), newRefresh);
        }
    }
}
