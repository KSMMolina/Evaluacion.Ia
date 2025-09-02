using Api_test_ia.Application.Abstractions.Persistence.Categories;
using Api_test_ia.Domain.Entities;
using Api_test_ia.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Api_test_ia.Infrastructure.Repositories.Categories
{
    public sealed class EfCategoryCommands(AppDbContext db) : ICategoryCommands
    {
        public Task<bool> NameExistsAsync(string name, int? excludingId, CancellationToken ct)
        => db.Categories.AnyAsync(c => c.Name == name && (!excludingId.HasValue || c.Id != excludingId), ct);

        public Task<bool> ExistsAsync(int id, CancellationToken ct)
            => db.Categories.AnyAsync(c => c.Id == id, ct);

        public async Task<bool> IsDescendantAsync(int id, int possibleParentId, CancellationToken ct)
        {
            var map = await db.Categories.AsNoTracking()
                .Select(c => new { c.Id, c.ParentCategoryId })
                .ToDictionaryAsync(x => x.Id, x => x.ParentCategoryId, ct);

            var current = map.GetValueOrDefault(possibleParentId);
            while (current is int parentId)
            {
                if (parentId == id) return true;
                current = map.GetValueOrDefault(parentId);
            }
            return false;
        }

        public Task<Category?> GetByIdAsync(int id, CancellationToken ct)
            => db.Categories.FirstOrDefaultAsync(c => c.Id == id, ct);

        public Task AddAsync(Category c, CancellationToken ct) { db.Categories.Add(c); return Task.CompletedTask; }
        public Task DeleteAsync(Category c, CancellationToken ct) { db.Categories.Remove(c); return Task.CompletedTask; }
    }
}
