namespace Ordernary.Models.DTOs
{
    public class CreteOrderDTO
    {
        public int TableId { get; set; }
        public List<CreateOrderArticleDTO> OrderArticles { get; set; }
    }
}
