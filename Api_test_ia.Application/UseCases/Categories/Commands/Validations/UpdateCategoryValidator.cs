using Api_test_ia.Application.Abstractions.Persistence.Categories;
using FluentValidation;

namespace Api_test_ia.Application.UseCases.Categories.Commands.Validations
{
    internal class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryValidator(ICategoryCommands repo)
        {
            When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
            {
                RuleFor(x => x.Name!)
                    .MustAsync(async (cmd, name, ct) => !await repo.NameExistsAsync(name!, cmd.Id, ct))
                    .WithMessage("El nombre ya existe.");
            });

            When(x => x.ParentCategoryId.HasValue, () =>
            {
                RuleFor(x => x.ParentCategoryId!.Value)
                    .MustAsync((id, ct) => repo.ExistsAsync(id, ct))
                    .WithMessage("La categoría padre no existe.");

                RuleFor(x => x)
                    .MustAsync(async (cmd, ct) => !await repo.IsDescendantAsync(cmd.Id, cmd.ParentCategoryId!.Value, ct))
                    .WithMessage("No puedes asignar como padre una categoría descendiente.");

                RuleFor(x => x)
                    .Must(cmd => cmd.ParentCategoryId != cmd.Id)
                    .WithMessage("La categoría no puede ser su propio padre.");
            });
        }
    }
}
