namespace API_PI_ADM_Clubes.Application.Interfaces.IServices
{
    public interface IImageService
    {
        Task<bool> DeleteImageAsync(Guid userId, string fileName, CancellationToken cancellationToken);
    }
}
