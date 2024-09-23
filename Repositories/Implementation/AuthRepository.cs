using Microsoft.EntityFrameworkCore;
using Ordernary.Data;
using Ordernary.Models;
using Ordernary.Repositories.Interface;
using System.Security.Cryptography;
using System.Text;

namespace Ordernary.Repositories.Implementation
{
    public class AuthRepository : IAuthRepository
    {
        private readonly Context _context;

        public AuthRepository(Context context)
        {
            _context = context;
        }

        public async Task<AppUser> RegisterAsync(AppUser user, string password)
        {
            if (await UserExistsAsync(user.Email))
                throw new Exception("User already exists");

            user.PasswordHash = HashPassword(password);

            _context.AppUsers.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<AppUser> LoginAsync(string email, string password)
        {
            var user = await _context.AppUsers.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null || !VerifyPassword(password, user.PasswordHash))
                throw new Exception("Invalid email or password");

            return user;
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.AppUsers.AnyAsync(u => u.Email == email);
        }

        private string HashPassword(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            using (var hmac = new HMACSHA512())
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return hashedPassword == Convert.ToBase64String(hash);
            }
        }
    }
}
