using Api_test_ia.Application.Common;
using Microsoft.EntityFrameworkCore;

namespace Api_test_ia.Infrastructure.Repositories
{
    public static class QueryableExtensions
    {
        public static async Task<PagedResult<T>> ToPagedAsync<T>(this IQueryable<T> query, int page, int pageSize, CancellationToken ct = default)
        {
            page = page <= 0 ? 1 : page;
            pageSize = pageSize <= 0 ? 12 : pageSize;
            var total = await query.LongCountAsync(ct);
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
            return new PagedResult<T>(items, page, pageSize, total);
        }
    }
}
