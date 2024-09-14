namespace Ordernary.Models.DTOs
{
    public class DailyReportResponseDTO
    {
        public IEnumerable<DailyReportDTO> ArticleReport { get; set; }
        public decimal TotalEarnings { get; set; }
    }
}
