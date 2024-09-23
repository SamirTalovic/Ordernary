using Microsoft.EntityFrameworkCore;
using Ordernary.Data;
using Ordernary.Models;

namespace Ordernary.Repositories.Implementation
{
    public class IngredientRepository
    {
        private readonly Context _context;

        public IngredientRepository(Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ingredient>> GetAllAsync()
        {
            return await _context.Ingredients.ToListAsync();
        }

        public async Task<Ingredient> GetByIdAsync(int id)
        {
            return await _context.Ingredients.FindAsync(id);
        }

        public async Task AddAsync(Ingredient ingredient)
        {
            await _context.Ingredients.AddAsync(ingredient);
        }

        public async Task UpdateAsync(Ingredient ingredient)
        {
            _context.Ingredients.Update(ingredient);
        }

        public async Task DeleteAsync(int id)
        {
            var ingredient = await GetByIdAsync(id);
            if (ingredient != null)
            {
                _context.Ingredients.Remove(ingredient);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
