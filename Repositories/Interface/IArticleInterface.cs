using Ordernary.Models;

namespace Ordernary.Repositories.Interface
{
    public interface IArticleInterface
    {
        Task<IEnumerable<Article>> GetAllAsync();
        Task<Article> GetByIdAsync(int articleId);
        Task AddAsync(Article article);
        Task UpdateAsync(Article article);
        Task DeleteAsync(int articleId);
        Task SaveChangesAsync();
    }
}
