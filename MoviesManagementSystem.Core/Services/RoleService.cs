using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MoviesManagementSystem.Core.Dots.AuthDots;
using MoviesManagementSystem.Core.Errors;
using MoviesManagementSystem.Core.IServices;
using MoviesManagementSystem.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesManagementSystem.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<string> AddUserToRoleAsync(UserRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user is null || !await _roleManager.RoleExistsAsync(model.RoleName))
                throw new ServiceException(StatusCodes.Status400BadRequest, "Invalid user ID or Role");

            if (await _userManager.IsInRoleAsync(user, model.RoleName))
                throw new ServiceException(StatusCodes.Status400BadRequest, "User already assigned to this role");

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);

            return result.Succeeded ? string.Empty : throw new ServiceException(StatusCodes.Status400BadRequest, "Sonething went wrong");
        }
        public async Task<string> AddRoleAsync(RoleModel model)
        {
            var Existing_Role = await _roleManager.FindByNameAsync(model.RoleName);
            if (Existing_Role is not null) throw new ServiceException(StatusCodes.Status400BadRequest, "Role already exists.");

            var result = await _roleManager.CreateAsync(new IdentityRole(model.RoleName));

            return result.Succeeded ? $"Role '{model.RoleName}' created successfully." : throw new ServiceException(StatusCodes.Status400BadRequest, "Sonething went wrong");
        }
        public async Task<string> RemoveRoleAsync(RoleModel model)
        {
            var Existing_Role = await _roleManager.FindByNameAsync(model.RoleName);
            if (Existing_Role is null) throw new NotFoundException("Role not exists.");

            var result = await _roleManager.DeleteAsync(Existing_Role);

            return result.Succeeded ? $"Role '{model.RoleName}' Deleted successfully." : throw new ServiceException(StatusCodes.Status400BadRequest, "Sonething went wrong");
        }

        public async Task<string> UpdateRoleAsync(UpdateRoleModel model)
        {
            var role = await _roleManager.FindByNameAsync(model.OldRoleName);
            if (role is null) return "Original role does not exist.";

            if (await _roleManager.RoleExistsAsync(model.NewRoleName))
                return $"Role name '{model.NewRoleName}' already exists.";

            role.Name = model.NewRoleName;


            var result = await _roleManager.UpdateAsync(role);

            return result.Succeeded ? $"Role Updated successfully." : throw new ServiceException(StatusCodes.Status400BadRequest, "Sonething went wrong");

        }

        public async Task<string> ReplaceUserRolesAsync(ChangeUserRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return "User not found.";

            foreach (var role in model.roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    return $"Role '{role}' does not exist.";
                }
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!removeResult.Succeeded)
            {
                return "Failed to remove current roles.";
            }

            var addResult = await _userManager.AddToRolesAsync(user, model.roles);

            return addResult.Succeeded ? $"Roles Updated successfully." : "Sonething went wrong";
        }

        public async Task<IEnumerable<string>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles.Select(x => x.Name).ToListAsync();
            return roles;
        }

    }
}
