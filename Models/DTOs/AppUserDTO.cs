namespace Ordernary.Models.DTOs
{
    public class AppUserDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Role Role { get; set; }
        public string? RestaurantName { get; set; }
    }
}
