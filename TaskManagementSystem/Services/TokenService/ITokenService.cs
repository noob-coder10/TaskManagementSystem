using Microsoft.AspNetCore.Identity;

namespace TaskManagementSystem.Services.TokenService
{
    public interface ITokenService
    {
        string CreateJWT(IdentityUser user, List<string> roles);
    }
}
