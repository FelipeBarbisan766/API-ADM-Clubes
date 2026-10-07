namespace API_PI_ADM_Clubes.Application.Interfaces.IRepositories;

public interface IManagerRepository
{
    Task<Manager?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}