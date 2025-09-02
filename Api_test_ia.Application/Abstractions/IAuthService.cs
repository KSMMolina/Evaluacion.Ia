using Api_test_ia.Application.Dtos;

namespace Api_test_ia.Application.Abstractions
{
    public interface IAuthService
    {
        Task<(AuthTokens tokens, string refreshToken)> LoginAsync(string email, string password, CancellationToken ct);
        Task<(AuthTokens tokens, string refreshToken)> RefreshAsync(string refreshToken, CancellationToken ct);
        Task LogoutAsync(string refreshToken, CancellationToken ct);
    }
}
