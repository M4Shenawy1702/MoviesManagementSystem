using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesManagementSystem.Core.Dots.AuthDots;
using MoviesManagementSystem.Core.Interfaces.Services;
using MoviesManagementSystem.Core.IServices;
using System.Threading.Tasks;

namespace MoviesManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAdminService _adminService;

        public AdminController(IAuthService authService, IAdminService adminService)
        {
            _authService = authService;
            _adminService = adminService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(string userId)
        {
            var user = await _adminService.GetUserAsync(userId);

            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _adminService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdmin([FromForm] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(dto, "Admin");

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(GetUser), new { userId = result.UserId }, result);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _authService.DeleteUserAsync(userId);

            if (!result.IsAuthenticated)
                return NotFound(result.Message);

            return NoContent();
        }
    }
}
