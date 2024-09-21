namespace Ordernary.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public Table Table { get; set; }
        public int TableId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderArticle> OrderArticles { get; set; }
        public bool Done { get; set; }
        public bool IsOnline { get; set; }
        public bool IsPickUp { get; set; }
        public string CustomerName { get; set; } 
        public string DeliveryAddress { get; set; }
    }
}
