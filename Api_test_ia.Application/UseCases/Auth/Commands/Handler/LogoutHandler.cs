using Api_test_ia.Application.Abstractions.Persistence.Auth;
using MediatR;

namespace Api_test_ia.Application.UseCases.Auth.Commands.Handler
{
    internal sealed class LogoutHandler(IRefreshTokenStore store) : IRequestHandler<LogoutCommand>
    {
        public async Task Handle(LogoutCommand r, CancellationToken ct)
        {
            var stored = await store.FindValidAsync(r.RefreshToken, ct);
            if (stored != null)
                await store.RevokeAsync(stored, ct);
        }
    }
}
