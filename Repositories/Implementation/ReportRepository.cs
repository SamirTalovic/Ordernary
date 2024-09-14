using Microsoft.EntityFrameworkCore;
using Ordernary.Data;
using Ordernary.Models.DTOs;
using Ordernary.Repositories.Interface;

namespace Ordernary.Repositories.Implementation
{
    public class ReportRepository : IReportInterface
    {
        private readonly Context _context;

        public ReportRepository(Context context)
        {
            _context = context;
        }

        public async Task<DailyReportResponseDTO> GetDailyReportAsync(DateTime date)
        {
            var articleReport = await _context.OrderArticles
                .Where(oa => oa.Order.CreatedAt.Date == date.Date)
                .GroupBy(oa => new { oa.ArticleId, oa.Article.Name })
                .Select(g => new DailyReportDTO
                {
                    ArticleName = g.Key.Name,
                    TotalQuantity = g.Sum(oa => oa.Quantity),
                    TotalPrice = g.Sum(oa => oa.Quantity * oa.Article.Price)
                })
                .ToListAsync();

            var totalEarnings = articleReport.Sum(r => r.TotalPrice);

            return new DailyReportResponseDTO
            {
                ArticleReport = articleReport,
                TotalEarnings = totalEarnings
            };
        }

    }
}
