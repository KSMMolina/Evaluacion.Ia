using Api_test_ia.Application.Abstractions.Persistence.Users;
using Api_test_ia.Application.Common;
using Api_test_ia.Application.Dtos;
using Api_test_ia.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Api_test_ia.Infrastructure.Repositories.Users
{
    public sealed class EfUserQueries(AppDbContext db) : IUserQueries
    {
        public async Task<PagedResult<UserDto>> ListAsync(string? search, string? role, string? sort, string? dir, int page, int pageSize, CancellationToken ct)
        {
            var q = db.Users.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(search)) q = q.Where(u => u.Email.Contains(search));
            if (!string.IsNullOrWhiteSpace(role)) q = q.Where(u => u.Role == role);

            var sortKey = (sort ?? "id").Trim().ToLowerInvariant();
            var asc = string.Equals((dir ?? "asc").Trim(), "asc", StringComparison.OrdinalIgnoreCase);
            q = sortKey switch
            {
                "email"     => asc ? q.OrderBy(u => u.Email).ThenBy(u => u.Id)
                                   : q.OrderByDescending(u => u.Email).ThenByDescending(u => u.Id),
                "createdat" => asc ? q.OrderBy(u => u.CreatedAt).ThenBy(u => u.Id)
                                   : q.OrderByDescending(u => u.CreatedAt).ThenByDescending(u => u.Id),
                _           => asc ? q.OrderBy(u => u.Id) : q.OrderByDescending(u => u.Id)
            };

            var total = await q.LongCountAsync(ct);
            var items = await q
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserDto(u.Id, u.Email, u.Role, u.CreatedAt))
                .ToListAsync(ct);

            return new PagedResult<UserDto>(items, page, pageSize, total);
        }

        public async Task<UserDetailDto?> GetAsync(int id, CancellationToken ct)
        {
            var u = await db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
            return u is null ? null : new UserDetailDto(u.Id, u.Email, u.Role, u.CreatedAt);
        }
    }
}
