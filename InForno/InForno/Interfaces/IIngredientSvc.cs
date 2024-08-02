using InForno.Models;
using InForno.Models.DTO;

namespace InForno.Services
{
    /// <summary>
    /// Interfaccia per il servizio degli ingredienti
    /// </summary>
    public interface IIngredientSvc
    {
        /// <summary>
        /// Ottiene tutti gli ingredienti
        /// </summary>
        /// <returns>Lista di Ingredient</returns>
        Task<List<Ingredient>> GetAllIngredients();

        /// <summary>
        /// Ottiene un ingrediente per ID
        /// </summary>
        /// <param name="id">ID dell'ingrediente</param>
        /// <returns>Ingrediente</returns>
        Task<Ingredient> GetIngredientById(int id);

        /// <summary>
        /// Ottiene gli ingredienti per una lista di ID
        /// </summary>
        /// <param name="ingredientIds">Lista di ID degli ingredienti</param>
        /// <returns>Lista di Ingredient</returns>
        Task<List<Ingredient>> GetIngredientsByIds(List<int> ingredientIds);

        /// <summary>
        /// Aggiunge un nuovo ingrediente
        /// </summary>
        /// <param name="model">Dati dell'ingrediente</param>
        /// <returns>Ingrediente aggiunto</returns>
        Task<Ingredient> AddIngredient(AddIngredientDTO model);

        /// <summary>
        /// Aggiorna un ingrediente esistente
        /// </summary>
        /// <param name="model">Dati dell'ingrediente</param>
        /// <returns>Ingrediente aggiornato</returns>
        Task<Ingredient> UpdateIngredient(UpdateIngredientDTO model);

        /// <summary>
        /// Elimina un ingrediente
        /// </summary>
        /// <param name="id">ID dell'ingrediente da eliminare</param>
        /// <returns>True se l'eliminazione ha successo, altrimenti false</returns>
        Task<bool> DeleteIngredient(int id);
    }
}
