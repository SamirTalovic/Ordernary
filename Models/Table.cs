namespace Ordernary.Models
{
    public class Table
    {
        public int TableId { get; set; }
        public bool Occupied { get; set; }
        public List<Order>? Orders { get; set; }
        public bool WeiterCall { get; set; }
        public int Number { get; set; }
        public int NumOfSeats { get; set; }
        public bool SmokingArea { get; set; }
    }
}
