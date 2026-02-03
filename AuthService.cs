using GamingPlatformAPI.DTO;
using GamingPlatformAPI.iService;
using GamingPlatformAPI.models;
using GamingPlatformAPI.ORM;
using Microsoft.EntityFrameworkCore;

namespace GamingPlatformAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly GamingDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthService(GamingDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Agents.AnyAsync(a => a.Email == email);
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto request)
        {
            var agent = await _context.Agents
                 .FirstOrDefaultAsync(a => a.Email == request.Email);

            if (agent == null)
            {
                return null;
            }

            // Verify password
            if (!BCrypt.Net.BCrypt.Verify(request.Password, agent.PasswordHash))
            {
                return null;
            }

           

            // Generate JWT token
            var token = _tokenService.GenerateToken(agent);
            var expiresAt = DateTime.UtcNow.AddMinutes(60);

            return new LoginResponseDto
            {
                AgentId = agent.Id,
                FullName = agent.Name,
                Email = agent.Email,
                Token = token,
                ExpiresAt = expiresAt
            };
        }

        public async Task<LoginResponseDto?> RegisterAsync(RegisterRequestDto request)
        {
            // Check if email already exists
            if (await EmailExistsAsync(request.Email))
            {
                return null;
            }

            // Hash password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var agent = new Agent
            {
                Name = request.FullName,
                Email = request.Email,
                PasswordHash = passwordHash,
             
            };

            _context.Agents.Add(agent);
            await _context.SaveChangesAsync();

            // Generate token for auto-login
            var token = _tokenService.GenerateToken(agent);
            var expiresAt = DateTime.UtcNow.AddMinutes(60);

            return new LoginResponseDto
            {
                AgentId = agent.Id,
                FullName = agent.Name,
                Email = agent.Email,
                Token = token,
                ExpiresAt = expiresAt
                
            };
        }
    }
}
