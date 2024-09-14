namespace Ordernary.Models.DTOs
{
    public class ArticleDTO
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public string Category { get; set; }
        public IFormFile Photo { get; set; }
    }
}
