namespace Api_test_ia.Presentation.Contracts.Admin.Users
{
    public sealed record CreateUserRequest(string Email, string Password, string Role);
}
