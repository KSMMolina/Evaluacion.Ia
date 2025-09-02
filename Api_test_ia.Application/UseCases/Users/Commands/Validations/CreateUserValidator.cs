using Api_test_ia.Application.Abstractions.Persistence.Users;
using FluentValidation;

namespace Api_test_ia.Application.UseCases.Users.Commands.Validations
{
    internal sealed class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserValidator(IUserCommands repo)
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(150)
                .MustAsync(async (email, ct) => !await repo.EmailExistsAsync(email, null, ct))
                .WithMessage("El email ya está registrado");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).MaximumLength(100);
            RuleFor(x => x.Role).NotEmpty().Must(r => r is "Admin" or "Editor")
                .WithMessage("Role inválido (Admin/Editor)");
        }
    }
}
