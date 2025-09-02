namespace Api_test_ia.Presentation.Contracts.Admin.Categories
{
    public sealed record CreateCategoryRequest(string Name, int? ParentCategoryId, bool? IsActive);
}
