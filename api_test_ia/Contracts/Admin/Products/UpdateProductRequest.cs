namespace Api_test_ia.Presentation.Contracts.Admin.Products
{
    public sealed record UpdateProductRequest(
    string? Name, string? Description, decimal? Price, int? CategoryId, bool? IsActive);
}
