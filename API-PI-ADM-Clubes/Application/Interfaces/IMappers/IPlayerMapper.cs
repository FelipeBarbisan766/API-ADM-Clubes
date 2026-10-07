using API_PI_ADM_Clubes.Application.DTOs;
using API_PI_ADM_Clubes.Model;

namespace API_PI_ADM_Clubes.Application.Interfaces.IMappers
{
    public interface IPlayerMapper
    {
        ResponsePlayerDTO ToDTO(Player player);
        IEnumerable<ResponsePlayerDTO> ToDTO(IEnumerable<Player> players);
    }
}
