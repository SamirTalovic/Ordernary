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
        public IActionResult Create(AppUserDTO userDto)
        {
            if (_context.AppUsers.Any(u => u.Email == userDto.Email))
            {
                return Ok("User already exists");
            }

            var newUser = new AppUser
            {
                Name = userDto.Name,
                Surname = userDto.Surname,
                Email = userDto.Email,
                PasswordHash = userDto.PasswordHash, // Ensure password hashing is done securely
                Role = userDto.Role
            };

            if (userDto.Role == Role.ADMIN)
            {
                var newRestaurant = new Restaurant
                {
                    Name = userDto.RestaurantName, // Assuming the restaurant name is passed in DTO
                    Owner = newUser
                };

                _context.Restaurants.Add(newRestaurant);
                newUser.Restaurant = newRestaurant;
            }

            _context.AppUsers.Add(newUser);
            _context.SaveChanges();

            return Ok("User and Restaurant created successfully");
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
                    userAvailable.Role.ToString(),
                    userAvailable.RestaurantId.ToString()
                    )) ;
            }
            return Ok("Failure");
        }
    }
}
