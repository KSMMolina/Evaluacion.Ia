using Api_test_ia.Application.Abstractions.Persistence.Auth;
using Api_test_ia.Application.Abstractions.Security;
using Api_test_ia.Application.Dtos;
using MediatR;

namespace Api_test_ia.Application.UseCases.Auth.Commands.Handler
{
    internal sealed class LoginHandler(IUserRepository users, IPasswordHasher hasher, IJwtProvider jwt, IRefreshTokenStore store)
    : IRequestHandler<LoginCommand, (AuthTokens tokens, string refreshToken)>
    {
        public async Task<(AuthTokens tokens, string refreshToken)> Handle(LoginCommand r, CancellationToken ct)
        {
            var u = await users.FindByEmailAsync(r.Email, ct) ?? throw new UnauthorizedAccessException();

            // soporta semilla en claro y hash
            var ok = u.PasswordHash == r.Password || hasher.Verify(r.Password, u.PasswordHash);
            if (!ok) throw new UnauthorizedAccessException();

            var access = jwt.CreateAccessToken(u);
            var (refresh, exp) = jwt.CreateRefreshToken();
            await store.AddAsync(u.Id, hasher.Hash(refresh), exp, ct);
            return (new AuthTokens(access, 15 * 60), refresh);
        }
    }
}
