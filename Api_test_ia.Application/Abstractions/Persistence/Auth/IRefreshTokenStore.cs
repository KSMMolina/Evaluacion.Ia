using Api_test_ia.Domain.Entities;

namespace Api_test_ia.Application.Abstractions.Persistence.Auth
{
    public interface IRefreshTokenStore
    {
        Task<UserRefreshToken> AddAsync(int userId, string tokenHash, DateTime expiresAt, CancellationToken ct);
        Task<UserRefreshToken?> FindValidAsync(string token, CancellationToken ct);
        Task RevokeAsync(UserRefreshToken token, CancellationToken ct);
        Task RevokeAllAsync(int userId, CancellationToken ct);
    }
}
