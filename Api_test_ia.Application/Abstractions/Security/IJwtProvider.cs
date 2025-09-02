using Api_test_ia.Domain.Entities;

namespace Api_test_ia.Application.Abstractions.Security
{
    public interface IJwtProvider
    {
        string CreateAccessToken(User user);
        (string token, DateTime expiresAt) CreateRefreshToken();
    }
}
