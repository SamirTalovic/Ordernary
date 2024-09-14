using Microsoft.EntityFrameworkCore;
using Ordernary.Data;
using Ordernary.Models;
using Ordernary.Repositories.Interface;

namespace Ordernary.Repositories.Implementation
{
    public class OrderArticleRepository : IOrderArticleInterface
    {
        private readonly Context _context;

        public OrderArticleRepository(Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderArticle>> GetAllAsync()
        {
            return await _context.OrderArticles
                .Include(oa => oa.Order)
                .Include(oa => oa.Article)
                .ToListAsync();
        }

        public async Task<OrderArticle> GetByIdAsync(int orderId, int articleId)
        {
            return await _context.OrderArticles
                .Include(oa => oa.Order)
                .Include(oa => oa.Article)
                .FirstOrDefaultAsync(oa => oa.OrderId == orderId && oa.ArticleId == articleId);
        }

        public async Task DeleteAsync(int orderId, int articleId)
        {
            var orderArticle = await GetByIdAsync(orderId, articleId);
            if (orderArticle != null)
            {
                _context.OrderArticles.Remove(orderArticle);
                await _context.SaveChangesAsync();
            }
        }
    }
}
