using API_PI_ADM_Clubes.Application.DTOs;

namespace API_PI_ADM_Clubes.Application.Hubs;

public interface ICourtAvailabilityHubClient
{
    Task ReserveStatusChanged(ReserveAvailabilityChangedDTO dto);
}