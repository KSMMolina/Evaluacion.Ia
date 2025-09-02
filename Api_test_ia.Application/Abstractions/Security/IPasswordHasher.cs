namespace Api_test_ia.Application.Abstractions.Security
{
    public interface IPasswordHasher
    {
        string Hash(string raw);
        bool Verify(string raw, string hash);
    }
}
