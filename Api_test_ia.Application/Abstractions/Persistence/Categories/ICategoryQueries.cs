using Api_test_ia.Application.Dtos;

namespace Api_test_ia.Application.Abstractions.Persistence.Categories
{
    public interface ICategoryQueries
    {
        Task<List<CategoryNodeDto>> GetTreeAsync(bool flat, bool onlyActive, CancellationToken ct);
    }
}
