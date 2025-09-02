using Api_test_ia.Application.UseCases.Products.Commands;
using Api_test_ia.Presentation.Contracts.Admin.Products;

namespace Api_test_ia.Presentation.Mappings
{
    public static class ProductsMappings
    {
        public static CreateProductCommand ToCommand(this CreateProductRequest r) =>
            new(r.Sku, r.Name, r.Price, r.Description, r.CategoryId, r.IsActive);

        public static UpdateProductCommand ToCommand(this int id, UpdateProductRequest r) =>
            new(id, r.Name, r.Description, r.Price, r.CategoryId, r.IsActive);
    }
}
