namespace Ordernary.Models
{
 
        public enum Role
        {
            WEITER,
            ADMIN
        }

        public class AppUser
        {
            public int AppUserId { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Email { get; set; }
            public string PasswordHash { get; set; }
            public Role Role { get; set; }
            public List<Table>? Tables { get; set; } 
            public List<Article>? CreatedArticles { get; set; }
            public int? RestaurantId{ get; set; }
            public Restaurant? Restaurant { get; set; }
    }
    }

