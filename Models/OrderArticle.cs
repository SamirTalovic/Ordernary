namespace Ordernary.Models
{
    public class OrderArticle
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public int Quantity { get; set; }
    }
}
