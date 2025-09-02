using Api_test_ia.Domain.Entities;

namespace Api_test_ia.Application.Abstractions.Persistence.Auth
{
    public interface IUserRepository
    {
        Task<User?> FindByEmailAsync(string email, CancellationToken ct);
        Task<User?> FindByIdAsync(int id, CancellationToken ct);
    }
}
