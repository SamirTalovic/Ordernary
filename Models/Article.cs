namespace Ordernary.Models
{
    public class Article
    {
        public int ArticleId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public List<OrderArticle> OrderArticles { get; set; }

    }
}
