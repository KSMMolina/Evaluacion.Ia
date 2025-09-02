using Api_test_ia.Application.Abstractions.Persistence.Categories;
using FluentValidation;

namespace Api_test_ia.Application.UseCases.Categories.Commands.Validations
{
    internal sealed class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryValidator(ICategoryCommands repo)
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100)
                .MustAsync(async (name, ct) => !await repo.NameExistsAsync(name, null, ct))
                .WithMessage("El nombre ya existe.");

            When(x => x.ParentCategoryId.HasValue, () =>
            {
                RuleFor(x => x.ParentCategoryId!.Value)
                    .MustAsync((id, ct) => repo.ExistsAsync(id, ct))
                    .WithMessage("La categoría padre no existe.");
            });
        }
    }
}
