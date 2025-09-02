namespace Api_test_ia.Presentation.Contracts.Admin.Categories
{
    public sealed record UpdateCategoryRequest(string? Name, int? ParentCategoryId, bool? IsActive);
}
