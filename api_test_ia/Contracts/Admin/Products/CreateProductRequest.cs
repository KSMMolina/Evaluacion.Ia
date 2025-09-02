namespace Api_test_ia.Presentation.Contracts.Admin.Products
{
    public sealed record CreateProductRequest(
    string Sku, string Name, decimal Price, string? Description, int? CategoryId, bool? IsActive);
}
