using API_PI_ADM_Clubes.Application.DTOs;

namespace API_PI_ADM_Clubes.Application.Interfaces.IServices
{
    public interface IClubService
    {
        Task<PagedResultDTO<ResponseClubDTO>> GetAll(ClubQueryDTO query, CancellationToken cancellationToken);
        Task<ResponseClubByIdDTO> GetById(Guid id, CancellationToken cancellationToken);
        Task<List<ResponseClubDTO>> GetAllByAdminId(Guid id, CancellationToken cancellationToken);
        Task<ResponseDashboardDTO> GetDashboard(Guid id, CancellationToken cancellationToken);
        Task<ResponseClubDTO> Update(Guid userId, Guid id, UpdateClubDTO dto, CancellationToken cancellationToken);
        Task Delete(Guid userId,Guid id, CancellationToken cancellationToken);
    }
}
