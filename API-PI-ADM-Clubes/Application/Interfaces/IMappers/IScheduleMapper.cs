using API_PI_ADM_Clubes.Application.DTOs;
using API_PI_ADM_Clubes.Model;

namespace API_PI_ADM_Clubes.Application.Interfaces.IMappers
{
    public interface IScheduleMapper
    {
        ResponseScheduleDTO ToDTO(Schedule schedule);
        IEnumerable<ResponseScheduleDTO> ToDTO(IEnumerable<Schedule> schedules);
        IEnumerable<ResponseScheduleAvailabilityDTO> ToAvailabilityDTO(IEnumerable<Schedule> schedules);
    }
}