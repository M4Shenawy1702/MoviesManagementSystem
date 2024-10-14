using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoviesManagementSystem.Core.Dots;
using MoviesManagementSystem.Core.Interfaces;
using MoviesManagementSystem.EF.Context;
using MoviesManagementSystem.EF.Models;

namespace MoviesManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private List<string> _AllowedExtensions = new List<string> { ".jpg", ".png" };
        private long _MaxAllowedSize = 10485760;


        public AuthController(IAuthRepository authRepository, UserManager<ApplicationUser> userManager, ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _authRepository = authRepository;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("UserRegister")]
        public async Task<IActionResult> RegisterAsync([FromForm] RegisterDto Dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authRepository.RegisterAsync(Dto, "NormalUser");

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] TokenRequestDto Dto)
        {
            var result = await _authRepository.GetTokenAsync(Dto);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPut("UpdateUserInfo/{UserId}")]
        public async Task<IActionResult> UpdateUserInfo(string UserId, [FromForm] RegisterDto Dto)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null) return NotFound();

            if (Dto.Email == user.Email)
                return BadRequest("Email already exists");
            else
            {

                user.FirstName = Dto.FirstName;
                user.LastName = Dto.LastName;
                user.Email = Dto.Email;
                user.Age = Dto.Age;
                user.BirtheDate = Dto.BirtheDate;
                user.UserName = Dto.UserName;
                user.PhoneNumber = Dto.PhoneNumber;


                if (Dto.ProfileImg is not null)
                {
                    var extension = Path.GetExtension(Dto.ProfileImg.FileName);
                    if (!_AllowedExtensions.Contains(extension.ToLower()))
                        return BadRequest("only .jpg and .png img are allowed");
                    if (Dto.ProfileImg.Length > _MaxAllowedSize)
                        return BadRequest("Max Allowed Size is 10Mb");

                    using var dataStream = new MemoryStream();
                    await Dto.ProfileImg.CopyToAsync(dataStream);

                    user.ProfileImg = dataStream.ToArray();
                }

                var RemovePass = await _userManager.RemovePasswordAsync(user);
                if (!RemovePass.Succeeded)
                    return BadRequest(RemovePass.Errors);
                var AddPass = await _userManager.AddPasswordAsync(user, Dto.Password);
                if (!AddPass.Succeeded)
                    return BadRequest(AddPass.Errors);
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                    return BadRequest(updateResult.Errors);

                else return Ok(user);
            }

        }

    }
}
