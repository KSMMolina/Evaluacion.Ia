namespace Api_test_ia.Domain.Entities
{
    public sealed class UserRefreshToken
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public string TokenHash { get; private set; } = default!;
        public DateTime ExpiresAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        private UserRefreshToken() { }
        public UserRefreshToken(int userId, string tokenHash, DateTime expiresAt) { UserId = userId; TokenHash = tokenHash; ExpiresAt = expiresAt; }
        public void Revoke() => RevokedAt = DateTime.UtcNow;
    }
}
