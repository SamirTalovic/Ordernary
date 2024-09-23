using Ordernary.Models;

namespace Ordernary.Repositories.Interface
{
    public interface IIngredientInterface
    {
        Task<IEnumerable<Ingredient>> GetAllAsync();
        Task<Ingredient> GetByIdAsync(int id);
        Task AddAsync(Ingredient ingredient);
        Task UpdateAsync(Ingredient ingredient);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
