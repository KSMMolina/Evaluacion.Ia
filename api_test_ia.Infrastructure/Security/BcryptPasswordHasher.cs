using Api_test_ia.Application.Abstractions.Security;

namespace Api_test_ia.Infrastructure.Security
{
    public sealed class BcryptPasswordHasher : IPasswordHasher
    {
        public string Hash(string raw) => BCrypt.Net.BCrypt.HashPassword(raw);
        public bool Verify(string raw, string hash) => BCrypt.Net.BCrypt.Verify(raw, hash);
    }
}
