using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordernary.Data;
using Ordernary.Models;
using Ordernary.Models.DTOs;
using Ordernary.Services;

namespace Ordernary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserControllers : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly Context _context;
        public UserControllers(IConfiguration config, Context context)
        {
            _config = config;
            _context = context;
        }
        [AllowAnonymous]
        [HttpPost("CreateUser")]
        public IActionResult Create(AppUser user)
        {

            if (_context.AppUsers.Where(u => u.Email == user.Email).FirstOrDefault() != null)
            {
                return Ok("Alredy exist");
            }

            _context.AppUsers.Add(user);
            _context.SaveChanges();

            return Ok("Created");
        }
        [AllowAnonymous]
        [HttpPost("LoginUser")]
        public IActionResult Login(Login user)
        {
            var userAvailable = _context.AppUsers.Where(u => u.Email == user.Email && u.PasswordHash == user.Pwd).FirstOrDefault();
            if (userAvailable != null)
            {
                return Ok(new TokenService(_config).GenerateToken(
                    userAvailable.AppUserId.ToString(),
                    userAvailable.Name,
                    userAvailable.Surname,
                    userAvailable.Email,
                    userAvailable.Role.ToString()
                    )) ;
            }
            return Ok("Failure");
        }
    }
}
