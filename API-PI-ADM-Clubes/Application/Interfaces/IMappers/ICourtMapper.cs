using API_PI_ADM_Clubes.Application.DTOs;
using API_PI_ADM_Clubes.Model;

namespace API_PI_ADM_Clubes.Application.Interfaces.IMappers
{
    public interface ICourtMapper
    {
        ResponseCourtDTO ToDTO(Court court);
        IEnumerable<ResponseCourtDTO> ToDTO(IEnumerable<Court> courts);
    }
}
