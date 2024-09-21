namespace Ordernary.Models.DTOs
{
    public class CreteOrderDTO
    {
        public int TableId { get; set; }
        public bool IsOnline { get; set; }
        public bool IsPickUp { get; set; }
        public List<CreateOrderArticleDTO> OrderArticles { get; set; }
    }
}
