using MoviesManagementSystem.Core.Dots.AuthDots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesManagementSystem.Core.IServices
{
    public interface IRoleService
    {
        Task<IEnumerable<string>> GetAllRolesAsync();
        Task<string> AddRoleAsync(RoleModel roleModel);
        Task<string> UpdateRoleAsync(UpdateRoleModel updateRoleModel);
        Task<string> RemoveRoleAsync(RoleModel roleModel);
        Task<string> AddUserToRoleAsync(UserRoleModel userRoleModel);
        Task<string> ReplaceUserRolesAsync(ChangeUserRoleModel changeUserRoleModel);
    }
}
