using Api_test_ia.Application.Abstractions.Persistence.Users;
using Api_test_ia.Domain.Entities;
using Api_test_ia.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Api_test_ia.Infrastructure.Repositories.Users
{
    public sealed class EfUserCommands(AppDbContext db) : IUserCommands
    {
        public Task<bool> EmailExistsAsync(string email, int? excludingUserId, CancellationToken ct)
            => db.Users.AnyAsync(u => u.Email == email && (!excludingUserId.HasValue || u.Id != excludingUserId), ct);

        public Task<User?> GetByIdAsync(int id, CancellationToken ct)
            => db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

        public Task AddAsync(User user, CancellationToken ct) { db.Users.Add(user); return Task.CompletedTask; }

        public Task DeleteAsync(User user, CancellationToken ct) { db.Users.Remove(user); return Task.CompletedTask; }
    }
}
