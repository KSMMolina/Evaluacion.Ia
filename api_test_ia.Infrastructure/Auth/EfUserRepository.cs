using Api_test_ia.Application.Abstractions.Persistence.Auth;
using Api_test_ia.Domain.Entities;
using Api_test_ia.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Api_test_ia.Infrastructure.Auth
{
    public sealed class EfUserRepository(AppDbContext db) : IUserRepository
    {
        public Task<User?> FindByEmailAsync(string email, CancellationToken ct)
        => db.Users.FirstOrDefaultAsync(x => x.Email == email, ct);

        public Task<User?> FindByIdAsync(int id, CancellationToken ct)
        => db.Users.FirstOrDefaultAsync(x => x.Id == id, ct);
    }
}
