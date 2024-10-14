using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MoviesManagementSystem.Core.Dots;
using MoviesManagementSystem.Core.Interfaces;
using MoviesManagementSystem.Core.Settings;
using MoviesManagementSystem.EF.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MoviesManagementSystem.Core.Models;
using MoviesManagementSystem.EF.Repositories;
using System.Numerics;
using MoviesManagementSystem.EF.Context;


namespace BLL.VotingSystem.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        private List<string> _AllowedExtensions = new List<string> { ".jpg", ".png" };
        private long _MaxAllowedSize = 10485760;
        public AuthRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IOptions<JWT> jwt, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
            _context = context;
        }

        public async Task<AuthModel> RegisterAsync(RegisterDto dto,string Role)
        {

            if (await _userManager.FindByEmailAsync(dto.Email) is not null)
                return new AuthModel { Message = "Email is already registered!" };

            if (await _userManager.FindByNameAsync(dto.UserName) is not null)
                return new AuthModel { Message = "Username is already registered!" };

                using var dataStream = new MemoryStream();
                await dto.ProfileImg.CopyToAsync(dataStream);

                var extension = Path.GetExtension(dto.ProfileImg.FileName);

                if (!_AllowedExtensions.Contains(extension.ToLower()))
                    return new AuthModel { Message = "only .jpg and .png img are allowed" };
                if (dto.ProfileImg.Length > _MaxAllowedSize)
                    return new AuthModel { Message = "Max Allowed Size is 10Mb" };
            
            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                ProfileImg = dataStream.ToArray(),
                Age = dto.Age,
                BirtheDate = dto.BirtheDate,
                Gender = dto.Gender,
                PhoneNumber = dto.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                if (Role == "NormalUser")
                {
                    var NormalUser = new NormalUser
                    {
                        UserId = user.Id,  
                    };
                    await _context.NormalUsers.AddAsync(NormalUser);
                    await _context.SaveChangesAsync();
                }
                else if (Role == "Admin")
                {
                    var Admin = new Admin
                    {
                        UserId = user.Id,
                    };
                    await _context.Admins.AddAsync(Admin);
                    await _context.SaveChangesAsync();

                }
                else if (Role == "SuperAdmin")
                {
                    var SuperAdmin = new SuperAdmin
                    {
                        UserId = user.Id,
                    };
                    await _context.SuperAdmins.AddAsync(SuperAdmin);
                    await _context.SaveChangesAsync();

                }
            }
            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                return new AuthModel { Message = errors };
            }

            await _userManager.AddToRoleAsync(user, Role);

            var jwtSecurityToken = await CreateJwtToken(user);

            

            return new AuthModel
            {
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { Role },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName,
            };
        }
        public async Task<AuthModel> DeleteAsync(string UserId, string Role)
        {
            var authModel = new AuthModel();

            var user = await _userManager.FindByIdAsync(UserId);
            if (user is null)
            {
                authModel.Message = "User Not Found!";
                return authModel;
            }


            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                return new AuthModel { Message = errors };
            }

            await _userManager.RemoveFromRoleAsync(user, Role);

            return new AuthModel
            {
                Email = user.Email,
                IsAuthenticated = true,
                Username = user.UserName,
                Message = "User deleted successfully",
            };
        }

        public async Task<AuthModel> GetTokenAsync(TokenRequestDto model)
        {
            var authModel = new AuthModel();

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Email or Password is incorrect!";
                return authModel;
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Roles = rolesList.ToList();

            return authModel;
        }

        public async Task<string> AddRoleAsync(AddRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user is null || !await _roleManager.RoleExistsAsync(model.Role))
                return "Invalid user ID or Role";

            if (await _userManager.IsInRoleAsync(user, model.Role))
                return "User already assigned to this role";

            var result = await _userManager.AddToRoleAsync(user, model.Role);
 
            return result.Succeeded ? string.Empty : "Sonething went wrong";
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
       
    }
}