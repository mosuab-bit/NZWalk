using Microsoft.AspNetCore.Identity;

namespace NZ_Walk.Repositories
{
    public interface ITokenRepository
    {
        string CreatJWTToken(IdentityUser user, List<string> roles);
    }
}
