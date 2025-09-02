namespace Api_test_ia.Application.Dtos;

public record CategoryListItemDto(
    int Id,
    string Name,
    int? ParentCategoryId,
    bool IsActive
);