using API_PI_ADM_Clubes.Application.DTOs;
using API_PI_ADM_Clubes.Application.Interfaces.IMappers;
using API_PI_ADM_Clubes.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API_PI_ADM_Clubes.Application.Mappers
{
    public class ReserveMapper : IReserveMapper
    {
        public ResponseReserveDTO ToDTO(Reserve reserve)
        {
            return new ResponseReserveDTO
            {
                Id = reserve.Id,
                Date = reserve.Date,
                Status = reserve.Status
            };
        }

        public IEnumerable<ResponseReserveDTO> ToDTO(IEnumerable<Reserve> reserves)
        {
            return reserves.Select(ToDTO);
        }
    }
}
