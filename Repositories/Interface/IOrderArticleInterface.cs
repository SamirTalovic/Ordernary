using Ordernary.Models;

namespace Ordernary.Repositories.Interface
{
    public interface IOrderArticleInterface
    {
        Task<IEnumerable<OrderArticle>> GetAllAsync();
        Task<OrderArticle> GetByIdAsync(int orderId, int articleId);
        Task DeleteAsync(int orderId, int articleId);

    }
}
