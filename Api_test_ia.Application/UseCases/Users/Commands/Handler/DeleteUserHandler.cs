using Api_test_ia.Application.Abstractions.Persistence;
using Api_test_ia.Application.Abstractions.Persistence.Auth;
using Api_test_ia.Application.Abstractions.Persistence.Users;
using MediatR;

namespace Api_test_ia.Application.UseCases.Users.Commands.Handler
{
    internal sealed class DeleteUserHandler(IUserCommands repo, IRefreshTokenStore tokens, IUnitOfWork uow)
    : IRequestHandler<DeleteUserCommand>
    {
        public async Task Handle(DeleteUserCommand r, CancellationToken ct)
        {
            var user = await repo.GetByIdAsync(r.Id, ct) ?? throw new KeyNotFoundException();
            await tokens.RevokeAllAsync(user.Id, ct);       // opcional: cierra sesiones
            await repo.DeleteAsync(user, ct);
            await uow.SaveChangesAsync(ct);
        }
    }
}
