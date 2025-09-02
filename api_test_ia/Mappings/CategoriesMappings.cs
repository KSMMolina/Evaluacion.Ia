using Api_test_ia.Application.UseCases.Categories.Commands;
using Api_test_ia.Presentation.Contracts.Admin.Categories;

namespace Api_test_ia.Presentation.Mappings
{
    public static class CategoriesMappings
    {
        public static CreateCategoryCommand ToCommand(this CreateCategoryRequest r) =>
            new(r.Name, r.ParentCategoryId, r.IsActive);

        public static UpdateCategoryCommand ToCommand(this int id, UpdateCategoryRequest r) =>
            new(id, r.Name, r.ParentCategoryId, r.IsActive);
    }
}
