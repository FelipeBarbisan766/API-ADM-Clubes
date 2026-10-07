// Infrastructure/Security/CookieAuthService.cs
using API_PI_ADM_Clubes.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using API_PI_ADM_Clubes.Application.Auth;

namespace API_PI_ADM_Clubes.Infrastructure.Security
{
    public class CookieAuthService : ICookieAuthService
    {
        public async Task SignInAsync(HttpContext httpContext, Manager manager)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, manager.Id.ToString()),
                new(ClaimTypes.Name, manager.Name),
                new(ClaimTypes.Email, manager.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true 
                });
        }

        public Task SignOutAsync(HttpContext httpContext) =>
            httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}