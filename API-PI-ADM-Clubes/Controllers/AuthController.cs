using API_PI_ADM_Clubes.Application.Auth;
using API_PI_ADM_Clubes.Application.DTOs;
using API_PI_ADM_Clubes.Application.Interfaces;
using API_PI_ADM_Clubes.Application.Interfaces.IServices;
using API_PI_ADM_Clubes.Infrastructure.Extensions;
using API_PI_ADM_Clubes.Infrastructure.Security;
using API_PI_ADM_Clubes.Infrastructure.Security.Interfaces;
using API_PI_ADM_Clubes.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_PI_ADM_Clubes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ICookieAuthService _cookieAuthService;

        public AuthController(IAuthService authService, ICookieAuthService cookieAuthService)
        {
            _authService = authService;
            _cookieAuthService = cookieAuthService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthDTO dto, CancellationToken cancellationToken)
        {
            try
            {
                var manager = await _authService.LoginAsync(dto, cancellationToken);
                await _cookieAuthService.SignInAsync(HttpContext, manager);
                return Ok("Login realizado com sucesso");
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
        [AllowAnonymous]
        public record GoogleSignUpRequest(string IdToken);

        [AllowAnonymous]
        [HttpPost("google/login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleSignUpRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var manager = await _authService.GoogleLogin(request.IdToken, cancellationToken);
                await _cookieAuthService.SignInAsync(HttpContext, manager);
                return Ok("Login realizado com sucesso");
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
        
    }
}