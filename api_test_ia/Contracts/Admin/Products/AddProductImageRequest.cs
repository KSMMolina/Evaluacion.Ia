namespace Api_test_ia.Presentation.Contracts.Admin.Products
{
    public sealed record AddProductImageRequest(string Url, string? AltText, int? SortOrder);
}
