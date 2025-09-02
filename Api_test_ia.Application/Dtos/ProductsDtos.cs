namespace Api_test_ia.Application.Dtos
{
    public partial record ProductImageDto(int Id, string Url, string? AltText, int SortOrder);
    public partial record ProductDetailDto(int Id, string Sku, string Name, string? Description, decimal Price, bool IsActive, int? CategoryId, DateTime CreatedAt, IReadOnlyList<ProductImageDto> Images);
}
