namespace Api_test_ia.Application.Dtos
{
    public sealed record ProductListItemDto
    {
        public ProductListItemDto(
            int id, string sku, string name, decimal price,
            int? categoryId, bool isActive, DateTime createdAt)
        {
            Id = id; Sku = sku; Name = name; Price = price;
            CategoryId = categoryId; IsActive = isActive; CreatedAt = createdAt;
        }

        public int Id { get; init; }
        public string Sku { get; init; } = null!;
        public string Name { get; init; } = null!;
        public decimal Price { get; init; }
        public int? CategoryId { get; init; }
        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
