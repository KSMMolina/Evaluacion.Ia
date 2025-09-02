using Api_test_ia.Application.UseCases.Auth.Commands;
using Api_test_ia.Presentation.Contracts.Auth;

namespace Api_test_ia.Presentation.Mappings
{
    public static class AuthMappings
    {
        public static LoginCommand ToCommand(this LoginRequest r) => new(r.Email, r.Password);
    }
}
