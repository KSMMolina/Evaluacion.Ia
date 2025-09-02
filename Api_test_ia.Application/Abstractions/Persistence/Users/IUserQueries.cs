using Api_test_ia.Application.Common;
using Api_test_ia.Application.Dtos;

namespace Api_test_ia.Application.Abstractions.Persistence.Users
{
    public interface IUserQueries
    {
        Task<PagedResult<UserDto>> ListAsync(string? search, string? role, string? sort, string? dir, int page, int pageSize, CancellationToken ct);
        Task<UserDetailDto?> GetAsync(int id, CancellationToken ct);
    }
}
