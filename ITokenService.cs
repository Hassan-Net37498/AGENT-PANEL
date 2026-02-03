using GamingPlatformAPI.models;

namespace GamingPlatformAPI.iService
{
    public interface ITokenService
    {
        string GenerateToken(Agent agent);
    }
}
