namespace Api_test_ia.Domain.Entities
{
    public sealed class Product
    {
        public int Id { get; private set; }
        public string Sku { get; private set; } = default!;
        public string Name { get; private set; } = default!;
        public string? Description { get; private set; }
        public decimal Price { get; private set; }
        public int? CategoryId { get; private set; }
        public bool IsActive { get; private set; } = true;
        public DateTime CreatedAt { get; private set; }

        public Category? Category { get; private set; }
        public ICollection<ProductImage> Images { get; private set; } = new List<ProductImage>();

        private Product() { }
        public Product(string sku, string name, decimal price, string? description, int? categoryId)
        {
            Sku = sku; Name = name; Price = price; Description = description; CategoryId = categoryId;
            CreatedAt = DateTime.UtcNow;
        }
        public void Update(string? name, string? description, decimal? price, int? categoryId, bool? isActive)
        {
            if (!string.IsNullOrWhiteSpace(name)) Name = name;
            Description = description ?? Description;
            if (price.HasValue) Price = price.Value;
            CategoryId = categoryId;
            if (isActive.HasValue) IsActive = isActive.Value;
        }
        public void Toggle() => IsActive = !IsActive;
    }
}