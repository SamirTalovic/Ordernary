namespace Ordernary.Models
{
    public class Table
    {
        public int TableId { get; set; }
        public bool Occupied { get; set; }
        public List<Order>? Orders { get; set; }
        public bool WeiterCall { get; set; }
        public int Number { get; set; }
        public bool SmokingArea { get; set; }
        public int? WeiterId { get; set; } 
        public AppUser? Weiter { get; set; }
    }
}
