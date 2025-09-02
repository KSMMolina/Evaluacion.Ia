using Api_test_ia.Domain.Entities;

namespace Api_test_ia.Application.Abstractions.Persistence.Users
{
    public interface IUserCommands
    {
        Task<bool> EmailExistsAsync(string email, int? excludingUserId, CancellationToken ct);
        Task<User?> GetByIdAsync(int id, CancellationToken ct);
        Task AddAsync(User user, CancellationToken ct);
        Task DeleteAsync(User user, CancellationToken ct);
    }
}
