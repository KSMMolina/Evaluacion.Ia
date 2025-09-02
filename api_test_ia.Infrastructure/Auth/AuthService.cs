using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api_test_ia.Application.Abstractions;
using Api_test_ia.Application.Dtos;
using Api_test_ia.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Api_test_ia.Domain.Entities;
using BCrypt.Net;
using System.Security.Cryptography;

namespace Api_test_ia.Infrastructure.Auth
{
    public sealed class AuthService(AppDbContext db, IOptions<JwtOptions> jwtOpt) : IAuthService
    {
        private readonly JwtOptions _jwt = jwtOpt.Value;

        public async Task<(AuthTokens tokens, string refreshToken)> LoginAsync(string email, string password, CancellationToken ct)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
            if (user is null) throw new UnauthorizedAccessException();

            // Para tu seed actual, acepta el claro "admin123" o hash si ya lo cambiaste.
            var ok = user.PasswordHash == password || BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!ok) throw new UnauthorizedAccessException();

            return await IssueTokensAsync(user, ct);
        }

        public async Task<(AuthTokens tokens, string refreshToken)> RefreshAsync(string refreshToken, CancellationToken ct)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(refreshToken);
            // búsqueda por coincidencia: mejor guarda el hash al crear y valida con Verify
            var stored = await db.Set<UserRefreshToken>().OrderByDescending(x => x.Id).FirstOrDefaultAsync(ct);
            if (stored is null || stored.RevokedAt != null || stored.ExpiresAt < DateTime.UtcNow || !BCrypt.Net.BCrypt.Verify(refreshToken, stored.TokenHash))
                throw new UnauthorizedAccessException();

            var user = await db.Users.FindAsync(new object?[] { stored.UserId }, ct) ?? throw new UnauthorizedAccessException();
            // rotación: revoca el anterior
            stored.Revoke();
            return await IssueTokensAsync(user, ct);
        }

        public async Task LogoutAsync(string refreshToken, CancellationToken ct)
        {
            var tokens = db.Set<UserRefreshToken>().AsQueryable();
            var all = await tokens.ToListAsync(ct);
            foreach (var t in all.Where(t => t.RevokedAt == null)) t.Revoke();
            await db.SaveChangesAsync(ct);
        }

        private async Task<(AuthTokens tokens, string refreshToken)> IssueTokensAsync(User user, CancellationToken ct)
        {
            var access = CreateJwt(user);
            var refresh = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var r = new UserRefreshToken(user.Id, BCrypt.Net.BCrypt.HashPassword(refresh), DateTime.UtcNow.AddDays(_jwt.RefreshTokenDays));
            db.Add(r);
            await db.SaveChangesAsync(ct);
            return (new AuthTokens(access, _jwt.AccessTokenMinutes * 60), refresh);
        }

        private string CreateJwt(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
        };
            var token = new JwtSecurityToken(_jwt.Issuer, _jwt.Audience, claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.AccessTokenMinutes), signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
