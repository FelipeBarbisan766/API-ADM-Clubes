using API_PI_ADM_Clubes.Application.DTOs;
using System.Security.Claims;
using API_PI_ADM_Clubes.Model;

namespace API_PI_ADM_Clubes.Application.Auth
{
    public interface IAuthService
    {
        Task<Manager> LoginAsync(AuthDTO dto, CancellationToken cancellationToken);       
        Task<Manager> GoogleLogin(string idToken, CancellationToken cancellationToken); 
       
    }
}
