using Ordernary.Models;

namespace Ordernary.Repositories.Interface
{
    public interface IAuthRepository
    {
        Task<AppUser> RegisterAsync(AppUser user, string password);
        Task<AppUser> LoginAsync(string email, string password);
        Task<bool> UserExistsAsync(string email);
    }
}
