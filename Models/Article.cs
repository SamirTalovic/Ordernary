namespace Ordernary.Models
{
    public class Article
    {
        public int ArticleId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public List<OrderArticle> OrderArticles { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public int? AdminId { get; set; } 
        public AppUser? Admin { get; set; }
        public List<Ingredient>? Ingredients { get; set; }
    }
}
