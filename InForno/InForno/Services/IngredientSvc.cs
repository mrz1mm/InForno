using InForno.Models;
using InForno.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace InForno.Services
{
    public class IngredientSvc
    {
        private readonly InFornoDbContext _context;

        public IngredientSvc(InFornoDbContext context)
        {
            _context = context;
        }

        public async Task<List<Ingredient>> GetAllIngredientsAsync()
        {
            return await _context.Ingredients.OrderBy(i => i.Name).ToListAsync();
        }

        public async Task<Ingredient> GetIngredientByIdAsync(int id)
        {
            return await _context.Ingredients.FindAsync(id);
        }

        public async Task<List<Ingredient>> GetIngredientsByIdsAsync(List<int> ingredientIds)
        {
            return await _context.Ingredients
                .Where(i => ingredientIds.Contains(i.IngredientId))
                .ToListAsync();
        }

        public async Task<Ingredient> AddIngredientAsync(AddIngredientDTO model)
        {
            var ingredient = new Ingredient
            {
                Name = model.Name
            };

            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();

            return ingredient;
        }

        public async Task<Ingredient> UpdateIngredientAsync(UpdateIngredientDTO model)
        {
            var ingredient = await _context.Ingredients.FindAsync(model.IngredientId);
            if (ingredient == null)
            {
                return null;
            }

            ingredient.Name = model.Name;

            _context.Ingredients.Update(ingredient);
            await _context.SaveChangesAsync();

            return ingredient;
        }

        public async Task<bool> DeleteIngredientAsync(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null)
            {
                return false;
            }

            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
