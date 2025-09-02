using Api_test_ia.Application.Abstractions.Persistence.Users;
using FluentValidation;

namespace Api_test_ia.Application.UseCases.Users.Commands.Validations
{
    internal sealed class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidator(IUserCommands repo)
        {
            When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
            {
                RuleFor(x => x.Email!).EmailAddress().MaximumLength(150)
                    .MustAsync(async (cmd, email, ct) => !await repo.EmailExistsAsync(email!, cmd.Id, ct))
                    .WithMessage("El email ya está registrado");
            });
            When(x => !string.IsNullOrWhiteSpace(x.Password), () =>
            {
                RuleFor(x => x.Password!).MinimumLength(6).MaximumLength(100);
            });
            When(x => !string.IsNullOrWhiteSpace(x.Role), () =>
            {
                RuleFor(x => x.Role!).Must(r => r is "Admin" or "Editor")
                    .WithMessage("Role inválido (Admin/Editor)");
            });
        }
    }
}
