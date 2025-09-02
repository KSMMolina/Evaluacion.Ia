using Api_test_ia.Application.Abstractions.Persistence;
using Api_test_ia.Application.Abstractions.Persistence.Users;
using Api_test_ia.Application.Abstractions.Security;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Api_test_ia.Application.UseCases.Users.Commands.Handler
{
    internal sealed class UpdateUserHandler(
    IUserCommands repo,
    IPasswordHasher hasher,
    IUnitOfWork uow,
    ILogger<UpdateUserHandler> logger)          // <- agrega logger por DI
    : IRequestHandler<UpdateUserCommand>
    {
        public async Task Handle(UpdateUserCommand r, CancellationToken ct)
        {
            try
            {
                // 1) Cargar usuario o 404
                var user = await repo.GetByIdAsync(r.Id, ct)
                           ?? throw new KeyNotFoundException($"Usuario {r.Id} no existe.");

                // 2) (Opcional) verificación previa de email duplicado para dar mensaje claro
                if (!string.IsNullOrWhiteSpace(r.Email) &&
                    await repo.EmailExistsAsync(r.Email!, r.Id, ct))
                {
                    throw new InvalidOperationException("El email ya está registrado.");
                }

                // 3) Actualizaciones
                user.Update(r.Email, r.Role);
                if (!string.IsNullOrWhiteSpace(r.Password))
                    user.SetPasswordHash(hasher.Hash(r.Password!));

                // 4) Guardar
                await uow.SaveChangesAsync(ct);
            }
            catch (KeyNotFoundException)
            {
                // 404: deja que el middleware/behavior lo traduzca
                logger.LogWarning("UpdateUser: usuario {UserId} no encontrado", r.Id);
                throw;
            }
            catch (Exception ex)
            {
                // 500
                logger.LogError(ex, "UpdateUser: error inesperado para usuario {UserId}", r.Id);
                throw;
            }
        }
    }
}
