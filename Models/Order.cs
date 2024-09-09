namespace Ordernary.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public Table Table { get; set; }
        public int TableId { get; set; }
        public List<OrderArticle> OrderArticles { get; set; }
        public bool Done { get; set; }
    }
}
