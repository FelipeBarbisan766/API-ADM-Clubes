using API_PI_ADM_Clubes.Application.DTOs;

namespace API_PI_ADM_Clubes.Application.Interfaces.IServices
{
    public interface ICourtService
    {
        Task<PagedResultDTO<ResponseCourtDTO>> GetAll(CourtQueryDTO query, CancellationToken cancellationToken);
        Task<ResponseCourtDTO> GetById(Guid id, CancellationToken cancellationToken);
        Task<List<ResponseCourtDTO>> GetByClubId(Guid id, CancellationToken cancellationToken);
        Task<ResponseCourtDTO> Update(Guid userId, Guid id, UpdateCourtDTO dto, CancellationToken cancellationToken);
        Task Delete(Guid userId, Guid id, CancellationToken cancellationToken);
    }
}
