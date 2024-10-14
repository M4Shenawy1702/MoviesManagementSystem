using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesManagementSystem.Core.Dots;
using MoviesManagementSystem.Core.Interfaces;

namespace MoviesManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperAdminController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        public SuperAdminController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        [HttpPost("AddSuperAdmin")]
        public async Task<IActionResult> AddSuperAdmin([FromForm] RegisterDto Dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authRepository.RegisterAsync(Dto, "SuperAdmin");

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }
        [HttpDelete("DeleteAdmin/{UserId}")]
        public async Task<IActionResult> DeleteAdmin(string UserId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authRepository.DeleteAsync(UserId, "Admin");

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}
