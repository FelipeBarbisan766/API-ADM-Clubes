using API_PI_ADM_Clubes.Application.DTOs;
using API_PI_ADM_Clubes.Application.Interfaces.IMappers;
using API_PI_ADM_Clubes.Model;

namespace API_PI_ADM_Clubes.Application.Mappers
{
    public class UserMapper : IUserMapper
    {
        public ResponseUserDTO ToDTO(User user)
        {
            return new ResponseUserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                AvatarUrl =  user.AvatarUrl,
                Role = user.Role,
                IsActive = user.IsActive,
                BirthDate = user.BirthDate,
                CreatedAt = user.CreatedAt,
                UpdatedAt =  user.UpdatedAt,
                Provider =  user.Provider
            };
        }

        public IEnumerable<ResponseUserDTO> ToDTO(IEnumerable<User> users)
        {
            return users.Select(ToDTO);
        }
    }
}
