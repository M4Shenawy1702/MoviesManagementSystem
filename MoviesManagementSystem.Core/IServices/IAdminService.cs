using MoviesManagementSystem.Core.Dots.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesManagementSystem.Core.IServices
{
    public interface IAdminService
    {
        Task<UserInfoDto> GetUserAsync(string userId);
        Task<IEnumerable<UserInfoDto>> GetAllUsersAsync();
    }
}
