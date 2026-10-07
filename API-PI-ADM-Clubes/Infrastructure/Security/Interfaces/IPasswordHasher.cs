namespace API_PI_ADM_Clubes.Infrastructure.Security.Interfaces
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string password, string hash);
    }
}
