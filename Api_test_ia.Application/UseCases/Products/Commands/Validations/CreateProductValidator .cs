using Api_test_ia.Application.Abstractions.Persistence.Products;
using FluentValidation;

namespace Api_test_ia.Application.UseCases.Products.Commands.Validations
{
    internal class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator(IProductCommands repo)
        {
            RuleFor(x => x.Sku).NotEmpty().MaximumLength(50)
                .MustAsync(async (sku, ct) => !await repo.SkuExistsAsync(sku, ct)).WithMessage("SKU ya existe");
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        }
    }
}
