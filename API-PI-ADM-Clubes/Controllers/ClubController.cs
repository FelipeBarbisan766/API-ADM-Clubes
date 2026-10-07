using API_PI_ADM_Clubes.Application.DTOs;
using API_PI_ADM_Clubes.Application.Interfaces.IServices;
using API_PI_ADM_Clubes.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_PI_ADM_Clubes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClubController : ControllerBase
    {
        private readonly IClubService _service;
        private readonly IAuthorizationService _authorizationService;
        public ClubController(IClubService service, IAuthorizationService authorizationService)
        {
            _service = service;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ClubQueryDTO query, CancellationToken cancellationToken)
        {
            var result = await _service.GetAll(query,cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _service.GetById(id, cancellationToken);
            return Ok(result);
        }
        
        [HttpGet("admin/{id}")]
        public async Task<IActionResult> GetAllByAdminId(Guid id, CancellationToken cancellationToken)
        {
            var result = await _service.GetAllByAdminId(id, cancellationToken);
            return Ok(result);
        }
        
        [HttpGet("{id}/dashboard")]
        public async Task<IActionResult> GetDashboard(Guid id, CancellationToken cancellationToken)
        {
            var result = await _service.GetDashboard(id, cancellationToken);
            return Ok(result);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateClubDTO dto, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId(); 
            var result = await _service.Update(userId,id, dto, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            await _service.Delete(userId,id,cancellationToken);
            return NoContent();
        }
        
        
    }
}
