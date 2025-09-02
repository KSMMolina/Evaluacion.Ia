namespace Api_test_ia.Application.Dtos
{
    public sealed record CategoryNodeDto(
    int Id,
    string Name,
    bool IsActive,
    int? ParentId,
    List<CategoryNodeDto> Children);
}
