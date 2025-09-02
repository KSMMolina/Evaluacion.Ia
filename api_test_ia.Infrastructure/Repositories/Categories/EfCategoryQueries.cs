using Api_test_ia.Application.Abstractions.Persistence.Categories;
using Api_test_ia.Application.Dtos;
using Api_test_ia.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Api_test_ia.Infrastructure.Repositories.Categories
{
    public sealed class EfCategoryQueries(AppDbContext db) : ICategoryQueries
    {
        public async Task<List<CategoryNodeDto>> GetTreeAsync(bool flat, bool onlyActive, CancellationToken ct)
        {
            var q = db.Categories.AsNoTracking().AsQueryable();
            if (onlyActive) q = q.Where(c => c.IsActive);

            var rows = await q.Select(c => new { c.Id, c.Name, c.IsActive, c.ParentCategoryId })
                              .OrderBy(c => c.Name)
                              .ToListAsync(ct);

            var dict = rows.ToDictionary(
                c => c.Id,
                c => new CategoryNodeDto(c.Id, c.Name, c.IsActive, c.ParentCategoryId, new List<CategoryNodeDto>()));

            foreach (var c in rows)
                if (c.ParentCategoryId is int pid && dict.TryGetValue(pid, out var parent))
                    parent.Children.Add(dict[c.Id]);

            var roots = dict.Values.Where(n => n.ParentId is null).ToList();
            if (flat) return dict.Values.OrderBy(n => n.Name).ToList();

            void Sort(List<CategoryNodeDto> nodes)
            {
                nodes.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));
                foreach (var n in nodes) Sort(n.Children);
            }
            Sort(roots);
            return roots;
        }
    }
}
