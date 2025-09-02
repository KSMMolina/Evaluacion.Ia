using FluentValidation;

namespace Api_test_ia.Application.UseCases.Products.Commands.Validations
{
    internal class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0).When(x => x.Price.HasValue);
        }
    }
}
