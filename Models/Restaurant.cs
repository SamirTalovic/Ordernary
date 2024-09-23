namespace Ordernary.Models
{
    public class Restaurant
    {
        public int? RestaurantId { get; set; }
        public string Name { get; set; }
        public AppUser Owner { get; set; }
        public int OwnerId { get; set; }
    }
}
