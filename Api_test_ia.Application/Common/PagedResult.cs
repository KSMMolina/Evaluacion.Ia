namespace Api_test_ia.Application.Common
{
    public sealed record PagedResult<T>(IReadOnlyList<T> Items, int Page, int PageSize, long Total);
}
