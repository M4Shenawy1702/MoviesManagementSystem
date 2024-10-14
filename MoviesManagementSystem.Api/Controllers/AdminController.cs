using BLL.VotingSystem.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesManagementSystem.Core.Dots;
using MoviesManagementSystem.Core.Interfaces;
using MoviesManagementSystem.EF.Context;
using MoviesManagementSystem.EF.Models;

namespace MoviesManagementSystem.Api.Controllers
{
    //[Authorize(Roles = "Admin")]
    //[Authorize(Roles = "SuperAdmin")]
    [ApiController]
    [Route("[controller]")]
    
    public class AdminController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private List<string> _AllowedExtensions = new List<string> { ".jpg", ".png" };
        private long _MaxAllowedSize = 10485760;


        public AdminController(IAuthRepository authRepository, UserManager<ApplicationUser> userManager, ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _authRepository = authRepository;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        [HttpPost("AddAdmin")]
        public async Task<IActionResult> AddAdmin([FromForm] RegisterDto Dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authRepository.RegisterAsync(Dto,"Admin");

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }
        [Authorize(Roles = "SuperAdmin")]


        //[HttpDelete("DeleteUser/{UserId}")]
        //public async Task<IActionResult> DeleteUser(string UserId)
        //{
        //    var user = await _userManager.FindByIdAsync(UserId);
        //    if (user == null)
        //        return NotFound();

        //    var result = await _userManager.DeleteAsync(user);

        //    if (!result.Succeeded)
        //    {
        //        return BadRequest("Failed to delete the user");
        //    }

        //    return Ok("User deleted successfully");
        //}
        [HttpDelete("DeleteNormalUser/{UserId}")]
        public async Task<IActionResult> DeleteNormalUser(string UserId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authRepository.DeleteAsync(UserId, "NormalUser");

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("addRole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authRepository.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);
        }
    }
}
