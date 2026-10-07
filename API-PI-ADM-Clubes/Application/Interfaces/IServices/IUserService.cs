using API_PI_ADM_Clubes.Application.DTOs;
using API_PI_ADM_Clubes.Model.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API_PI_ADM_Clubes.Application.Interfaces.IServices
{
    public interface IUserService
    {
        Task<ResponseUserDTO> GetById(Guid id, CancellationToken cancellationToken);
        Task<ResponseUserDTO> Update(Guid id, UpdateUserDTO dto, CancellationToken cancellationToken);
        Task UpdateRole(Guid id, RoleEnum role, CancellationToken cancellationToken);
        Task Delete(Guid id, CancellationToken cancellationToken);
    }
}
