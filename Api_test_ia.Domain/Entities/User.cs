namespace Api_test_ia.Domain.Entities
{
    public sealed class User
    {
        public int Id { get; private set; }
        public string Email { get; private set; } = default!;
        public string PasswordHash { get; private set; } = default!;
        public string Role { get; private set; } = default!; // "Admin" | "Editor"
        public DateTime CreatedAt { get; private set; }

        private User() { }
        public User(string email, string passwordHash, string role)
        {
            Email = email; PasswordHash = passwordHash; Role = role;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(string? email, string? role)
        {
            if (!string.IsNullOrWhiteSpace(email)) Email = email;
            if (!string.IsNullOrWhiteSpace(role)) Role = role;
        }
        public void SetPasswordHash(string passwordHash)
        {
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
        }

    }
}