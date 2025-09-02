using Api_test_ia.Application.Dtos;

namespace Api_test_ia.Application.Abstractions
{
    public interface ICategoriesService
    {
        Task<List<CategoryNodeDto>> GetTreeAsync(bool flat, CancellationToken ct);
        Task<int> CreateAsync(string name, int? parentId, bool? isActive, CancellationToken ct);
        Task UpdateAsync(int id, string? name, int? parentId, bool? isActive, CancellationToken ct);
        Task ToggleAsync(int id, CancellationToken ct);
    }
}
