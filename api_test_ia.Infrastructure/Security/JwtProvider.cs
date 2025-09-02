using Api_test_ia.Application.Abstractions.Security;
using Api_test_ia.Domain.Entities;
using Api_test_ia.Infrastructure.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Api_test_ia.Infrastructure.Security
{
    public sealed class JwtProvider(IOptions<JwtOptions> opt) : IJwtProvider
    {
        private readonly JwtOptions _o = opt.Value;
        public string CreateAccessToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_o.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };
            var token = new JwtSecurityToken(_o.Issuer, _o.Audience, claims,
                expires: DateTime.UtcNow.AddMinutes(_o.AccessTokenMinutes), signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public (string token, DateTime expiresAt) CreateRefreshToken()
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            return (token, DateTime.UtcNow.AddDays(_o.RefreshTokenDays));
        }
    }
}
