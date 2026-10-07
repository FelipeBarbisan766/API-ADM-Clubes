namespace API_PI_ADM_Clubes.Application.Interfaces.IServices;

public interface IReserveCleanupService
{
    Task<int> CleanupOldReservesAsync();
}