using Api_test_ia.Application.Abstractions.Persistence;
using Api_test_ia.Infrastructure.Persistence.Context;

namespace Api_test_ia.Infrastructure.Persistence.Uow
{
    public sealed class EfUnitOfWork(AppDbContext db) : IUnitOfWork
    {
        public Task<int> SaveChangesAsync(CancellationToken ct = default) => db.SaveChangesAsync(ct);
    }
}
