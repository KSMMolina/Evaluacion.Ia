using Api_test_ia.Application.Abstractions.Persistence.Auth;
using Api_test_ia.Domain.Entities;
using Api_test_ia.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Api_test_ia.Infrastructure.Auth
{
    public sealed class EfRefreshTokenStore(AppDbContext db) : IRefreshTokenStore
    {
        public async Task<UserRefreshToken> AddAsync(int userId, string tokenHash, DateTime expiresAt, CancellationToken ct)
        {
            var t = new UserRefreshToken(userId, tokenHash, expiresAt);
            db.Add(t); await db.SaveChangesAsync(ct); return t;
        }

        public async Task<UserRefreshToken?> FindValidAsync(string token, CancellationToken ct)
        {
            var now = DateTime.UtcNow;
            var candidates = await db.Set<UserRefreshToken>()
                .Where(x => x.RevokedAt == null && x.ExpiresAt > now)
                .OrderByDescending(x => x.Id)
                .ToListAsync(ct);

            foreach (var t in candidates)
            {
                if (BCrypt.Net.BCrypt.Verify(token, t.TokenHash))
                    return t;
            }
            return null;
        }

        public Task RevokeAsync(UserRefreshToken token, CancellationToken ct)
        { token.Revoke(); return db.SaveChangesAsync(ct); }

        public async Task RevokeAllAsync(int userId, CancellationToken ct)
        {
            var tokens = await db.Set<UserRefreshToken>()
                .Where(x => x.UserId == userId && x.RevokedAt == null)
                .ToListAsync(ct);
            tokens.ForEach(t => t.Revoke());
            await db.SaveChangesAsync(ct);
        }
    }
}
