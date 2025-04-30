using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesManagementSystem.Core.Dots.AuthDots;
using MoviesManagementSystem.Core.Dots.UserDtos;
using MoviesManagementSystem.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace MoviesManagementSystem.Controllers
{
    [Route("api/users/{userId}")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("likes/{movieId}")]
        public async Task<IActionResult> ToggleLikeAsync(string userId, int movieId)
        {
            var result = await _userService.ToggleLikeAsync(movieId, userId);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateInfoAsync(string userId, [FromForm] UpdateInfoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.UpdateInfoAsync(userId, dto);
            return Ok(result);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePasswordAsync(string userId, [FromBody] ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.ChangePasswordAsync(userId, dto);
            return Ok(result);
        }
    }
}
