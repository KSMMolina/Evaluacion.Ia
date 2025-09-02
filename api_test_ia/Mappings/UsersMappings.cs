using Api_test_ia.Application.UseCases.Users.Commands;
using Api_test_ia.Presentation.Contracts.Admin.Users;

namespace Api_test_ia.Presentation.Mappings
{
    public static class UsersMappings
    {
        public static CreateUserCommand ToCommand(this CreateUserRequest r) => new(r.Email, r.Password, r.Role);
        public static UpdateUserCommand ToCommand(this int id, UpdateUserRequest r) => new(id, r.Email, r.Password, r.Role);
    }
}
