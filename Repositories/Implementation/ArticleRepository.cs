using Microsoft.EntityFrameworkCore;
using Ordernary.Data;
using Ordernary.Models;
using Ordernary.Repositories.Interface;

namespace Ordernary.Repositories.Implementation
{
    public class ArticleRepository : IArticleInterface
    {
        private readonly Context _context;

        public ArticleRepository(Context context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Article>> GetAllAsync()
        {
            return await _context.Articles.Include(a => a.Ingredients).ToListAsync();
        }
        public async Task<Article> GetByIdAsync(int articleId)
        {
            return await _context.Articles.Include(a => a.Ingredients).FirstOrDefaultAsync(a => a.ArticleId == articleId);
        }
        public async Task AddAsync(Article article)
        {
            await _context.Articles.AddAsync(article);
        }
        public async Task UpdateAsync(Article article)
        {
            _context.Articles.Update(article);
        }
        public async Task DeleteAsync(int articleId)
        {
            var article = await GetByIdAsync(articleId);
            if (article != null)
            {
                _context.Articles.Remove(article);
            }
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
