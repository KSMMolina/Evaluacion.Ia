using Api_test_ia.Application.Abstractions.Persistence;
using Api_test_ia.Application.Abstractions.Persistence.Users;
using Api_test_ia.Application.Abstractions.Security;
using Api_test_ia.Domain.Entities;
using MediatR;

namespace Api_test_ia.Application.UseCases.Users.Commands.Handler
{
    internal sealed class CreateUserHandler(IUserCommands repo, IPasswordHasher hasher, IUnitOfWork uow)
    : IRequestHandler<CreateUserCommand, int>
    {
        public async Task<int> Handle(CreateUserCommand r, CancellationToken ct)
        {
            var hash = hasher.Hash(r.Password);
            var user = new User(r.Email, hash, r.Role);
            await repo.AddAsync(user, ct);
            await uow.SaveChangesAsync(ct);
            return user.Id;
        }
    }
}
