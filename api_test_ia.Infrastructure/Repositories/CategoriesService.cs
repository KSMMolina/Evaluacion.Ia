using Api_test_ia.Application.Abstractions;
using Api_test_ia.Application.Dtos;
using Api_test_ia.Domain.Entities;
using Api_test_ia.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Api_test_ia.Infrastructure.Repositories
{
    public sealed class CategoriesService(AppDbContext db) : ICategoriesService
    {
        public async Task<List<CategoryNodeDto>> GetTreeAsync(bool flat, CancellationToken ct)
        {
            var cats = await db.Categories.AsNoTracking().ToListAsync(ct);
            if (flat) return cats.Select(c => new CategoryNodeDto(c.Id, c.Name, c.IsActive, null, new List<CategoryNodeDto>())).ToList();

            var dict = cats.ToDictionary(c => c.Id, c => new CategoryNodeDto(c.Id, c.Name, c.IsActive, c.ParentCategoryId, new List<CategoryNodeDto>()));
            var roots = new List<CategoryNodeDto>();
            foreach (var c in cats)
            {
                if (c.ParentCategoryId is null) roots.Add(dict[c.Id]);
                else dict[c.ParentCategoryId.Value].Children!.Add(dict[c.Id]);
            }
            return roots;
        }

        public async Task<int> CreateAsync(string name, int? parentId, bool? isActive, CancellationToken ct)
        {
            if (await db.Categories.AnyAsync(c => c.Name == name, ct)) throw new InvalidOperationException("Nombre de categoría duplicado");
            var c = new Category(name, parentId, isActive ?? true);
            db.Categories.Add(c);
            await db.SaveChangesAsync(ct);
            return c.Id;
        }

        public async Task UpdateAsync(int id, string? name, int? parentId, bool? isActive, CancellationToken ct)
        {
            var c = await db.Categories.FindAsync(new object?[] { id }, ct) ?? throw new KeyNotFoundException();
            c.Update(name, parentId, isActive);
            await db.SaveChangesAsync(ct);
        }

        public async Task ToggleAsync(int id, CancellationToken ct)
        {
            var c = await db.Categories.FindAsync(new object?[] { id }, ct) ?? throw new KeyNotFoundException();
            c.Toggle();
            await db.SaveChangesAsync(ct);
        }
    }
}
