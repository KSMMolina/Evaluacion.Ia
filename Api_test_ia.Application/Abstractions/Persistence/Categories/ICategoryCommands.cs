using Api_test_ia.Domain.Entities;

namespace Api_test_ia.Application.Abstractions.Persistence.Categories
{
    public interface ICategoryCommands
    {
        Task<bool> NameExistsAsync(string name, int? excludingId, CancellationToken ct);
        Task<bool> ExistsAsync(int id, CancellationToken ct);
        Task<bool> IsDescendantAsync(int id, int possibleParentId, CancellationToken ct); // evitar ciclos

        Task<Category?> GetByIdAsync(int id, CancellationToken ct); // tracked
        Task AddAsync(Category category, CancellationToken ct);
        Task DeleteAsync(Category category, CancellationToken ct);
    }
}
