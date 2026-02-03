using GamingPlatformAPI.DTO;
using GamingPlatformAPI.iService;
using GamingPlatformAPI.Migrations;
using GamingPlatformAPI.models;
using GamingPlatformAPI.ORM;
using Microsoft.EntityFrameworkCore;

namespace GamingPlatformAPI.Service
{
    public class UserService : IUserService
    {
        private readonly GamingDbContext _context;
        public UserService( GamingDbContext context)
        {
            _context = context;
        }
        public async Task<List<user>> GetDataAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<int> CountAsync()
        {
            return await _context.Users.CountAsync();
        }


        public async Task<PaginatedResult<UserDto>> GetUsersAsync(int agentId, int page, int pageSize, string? search = null, bool? isBlocked = null)
        {
            var query = _context.Users.Where(u => u.AgentId == agentId);

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u =>
                    u.FullName.Contains(search) ||
                    u.Email.Contains(search) ||
                    (u.PhoneNumber != null && u.PhoneNumber.Contains(search))
                );
            }

            // Apply block status filter
            if (isBlocked.HasValue)
            {
                query = query.Where(u => u.IsBlocked == isBlocked.Value);
            }

            // Get total count
            var totalItems = await query.CountAsync();

            // Calculate pagination
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var skip = (page - 1) * pageSize;

            // Get paginated data
            var users = await query
                .OrderByDescending(u => u.CreatedAt)
                .Skip(skip)
                .Take(pageSize)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Country = u.Country,
                    TotalDeposits = u.TotalDeposits,
                    TotalWagers = u.TotalWagers,
                    TotalLosses = u.TotalLosses,
                    IsBlocked = u.IsBlocked,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();

            return new PaginatedResult<UserDto>
            {
                Items = users,
                TotalItems = totalItems,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                HasPrevious = page > 1,
                HasNext = page < totalPages
            };
        }

        public async Task<UserDto?> GetUserByIdAsync(int userId, int agentId)
        {
            var user = await _context.Users
                .Where(u => u.Id == userId && u.AgentId == agentId)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Country = u.Country,
                    TotalDeposits = u.TotalDeposits,
                    TotalWagers = u.TotalWagers,
                    TotalLosses = u.TotalLosses,
                    IsBlocked = u.IsBlocked,
                    CreatedAt = u.CreatedAt
                })
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<user?> CreateUserAsync(CreateUserDto dto, int agentId)
        {
            // Check if email already exists
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("Email already exists");

            var user = new user
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Country = dto.Country,
                AgentId = agentId,
                CreatedAt = DateTime.UtcNow,
                IsBlocked = false,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return  user;
        }

        public async Task<UserDto?> UpdateUserAsync(int userId, UpdateUserDto updateUserDto, int agentId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId && u.AgentId == agentId);

            if (user == null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(updateUserDto.FullName))
                user.FullName = updateUserDto.FullName;

            if (updateUserDto.PhoneNumber != null)
                user.PhoneNumber = updateUserDto.PhoneNumber;

            if (updateUserDto.Country != null)
                user.Country = updateUserDto.Country;

            user.CreatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Country = user.Country,
                TotalDeposits = user.TotalDeposits,
                TotalWagers = user.TotalWagers,
                TotalLosses = user.TotalLosses,
                IsBlocked = user.IsBlocked,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<bool> ToggleBlockStatusAsync(int userId, int agentId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId && u.AgentId == agentId);

            if (user == null)
            {
                return false;
            }

            user.IsBlocked = !user.IsBlocked;
            user.CreatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int userId, int agentId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId && u.AgentId == agentId);

            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
    
}
