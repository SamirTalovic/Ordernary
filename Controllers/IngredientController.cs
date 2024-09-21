using Microsoft.AspNetCore.Mvc;
using Ordernary.Models;
using Ordernary.Repositories.Interface;

namespace Ordernary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientInterface _ingredientRepository;

        public IngredientController(IIngredientInterface ingredientInterface)
        {
            _ingredientRepository = ingredientInterface;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ingredient>>> GetIngredients()
        {
            var ingredients = await _ingredientRepository.GetAllAsync();
            return Ok(ingredients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ingredient>> GetIngredient(int id)
        {
            var ingredient = await _ingredientRepository.GetByIdAsync(id);
            if (ingredient == null)
            {
                return NotFound();
            }
            return Ok(ingredient);
        }

        [HttpPost]
        public async Task<ActionResult<Ingredient>> PostIngredient(Ingredient ingredient)
        {
            await _ingredientRepository.AddAsync(ingredient);
            await _ingredientRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIngredient), new { id = ingredient.IngredientId }, ingredient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutIngredient(int id, Ingredient ingredient)
        {
            var existingIngredient = await _ingredientRepository.GetByIdAsync(id);
            if (existingIngredient == null)
            {
                return NotFound();
            }

            existingIngredient.Name = ingredient.Name;

            await _ingredientRepository.UpdateAsync(existingIngredient);
            await _ingredientRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngredient(int id)
        {
            await _ingredientRepository.DeleteAsync(id);
            await _ingredientRepository.SaveChangesAsync();
            return NoContent();
        }
    }
}
