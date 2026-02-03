using GamingPlatformAPI.DTO;

namespace GamingPlatformAPI.iService
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginDto request);
        Task<LoginResponseDto?> RegisterAsync(RegisterRequestDto request);
        Task<bool> EmailExistsAsync(string email);
    }
}
