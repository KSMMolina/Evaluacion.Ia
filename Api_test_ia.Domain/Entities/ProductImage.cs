namespace Api_test_ia.Domain.Entities
{
    public sealed class ProductImage
    {
        public int Id { get; private set; }
        public int ProductId { get; private set; }
        public string Url { get; private set; } = default!;
        public string? AltText { get; private set; }
        public int SortOrder { get; private set; }

        public Product Product { get; private set; } = default!;

        private ProductImage() { } // EF

        public ProductImage(int productId, string url, string? altText, int sortOrder = 0)
        {
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentException("url requerida", nameof(url));
            ProductId = productId;
            Url = url;
            AltText = altText;
            SortOrder = sortOrder;
        }

        public void Edit(string? altText, int? sortOrder)
        {
            AltText = altText ?? AltText;
            if (sortOrder.HasValue) SortOrder = sortOrder.Value;
        }

        // 👉 método de dominio para reemplazar el archivo (cambiar la URL)
        public void ReplaceUrl(string newUrl)
        {
            if (string.IsNullOrWhiteSpace(newUrl)) throw new ArgumentException("url requerida", nameof(newUrl));
            Url = newUrl;
        }
    }
}
