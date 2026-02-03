using GamingPlatformAPI.DTO;
using GamingPlatformAPI.models;
using Microsoft.EntityFrameworkCore;

namespace GamingPlatformAPI.iService
{
    public interface IUserService
    {
        Task<PaginatedResult<UserDto>> GetUsersAsync(int agentId, int page, int pageSize, string? search = null, bool? isBlocked = null);
        Task<UserDto?> GetUserByIdAsync(int userId, int agentId);
        Task<user> CreateUserAsync(CreateUserDto createUserDto, int agentId);
        Task<UserDto?> UpdateUserAsync(int userId, UpdateUserDto updateUserDto, int agentId);
        Task<bool> ToggleBlockStatusAsync(int userId, int agentId);
        Task<bool> DeleteUserAsync(int userId, int agentId);
        Task<List<user>> GetDataAsync();
        public  Task<int> CountAsync();

    }

}
