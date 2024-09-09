using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoviesManagementSystem.Core.Dots;
using MoviesManagementSystem.Core.Interfaces;
using MoviesManagementSystem.EF.Models;

namespace MoviesManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NormalUserController : ControllerBase
    {
        private readonly IAuthRepository _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private List<string> _AllowedExtensions = new List<string> { ".jpg", ".png" };
        private long _MaxAllowedSize = 10485760;
        public NormalUserController()
        {

        }

        [HttpPut("UpdateUserInfo/{UserId}")]
        public async Task<IActionResult> UpdateUserInfo(string UserId, [FromForm] RegisterDto Dto)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
                return NotFound();


            if (Dto.Email == user.Email)
                return BadRequest("Email already exists");
            else
            {

                user.FirstName = Dto.FirstName;
                user.LastName = Dto.LastName;
                user.Email = Dto.Email;
                user.Age = Dto.Age;
                user.BirtheDate = Dto.BirtheDate;
                user.UserName = Dto.Username;
                user.PhoneNumber = Dto.PhoneNumber;
                //user.Gender = Dto.Gender;

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

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded) return BadRequest(result.Errors);

                else return Ok(user);
            }
        }
    }
}