using Ordernary.Models;

namespace Ordernary.Services.ServiceInterfaces
{
    public interface ITokenService
    {
        string GenerateToken(AppUser user);
    }
}
