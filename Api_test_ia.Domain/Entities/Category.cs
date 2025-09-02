namespace Api_test_ia.Domain.Entities
{
    public sealed class Category
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = default!;
        public int? ParentCategoryId { get; private set; }
        public bool IsActive { get; private set; } = true;
        public DateTime CreatedAt { get; private set; }

        public Category? Parent { get; private set; }
        public ICollection<Category> Children { get; private set; } = new List<Category>();
        public ICollection<Product> Products { get; private set; } = new List<Product>();

        private Category() { }
        public Category(string name, int? parentCategoryId = null, bool isActive = true)
        {
            Name = name; ParentCategoryId = parentCategoryId; IsActive = isActive;
            CreatedAt = DateTime.UtcNow;
        }

        public void Toggle() => IsActive = !IsActive;
        public void Update(string? name, int? parentId, bool? isActive)
        {
            if (!string.IsNullOrWhiteSpace(name)) Name = name;
            ParentCategoryId = parentId;
            if (isActive.HasValue) IsActive = isActive.Value;
        }
    }

}
