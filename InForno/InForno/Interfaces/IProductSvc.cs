using InForno.Models;
using InForno.Models.DTO;

namespace InForno.Services
{
    /// <summary>
    /// Interfaccia per il servizio dei prodotti
    /// </summary>
    public interface IProductSvc
    {
        /// <summary>
        /// Ottiene tutti i prodotti
        /// </summary>
        /// <returns>Lista di Product</returns>
        Task<List<Product>> GetAllProducts();

        /// <summary>
        /// Ottiene un prodotto per ID
        /// </summary>
        /// <param name="id">ID del prodotto</param>
        /// <returns>Prodotto</returns>
        Task<Product> GetProductById(int id);

        /// <summary>
        /// Aggiunge un nuovo prodotto
        /// </summary>
        /// <param name="model">Dati del prodotto</param>
        /// <returns>Task completato</returns>
        Task AddProduct(AddProductDTO model);

        /// <summary>
        /// Aggiorna un prodotto esistente
        /// </summary>
        /// <param name="model">Dati del prodotto</param>
        /// <returns>Prodotto aggiornato</returns>
        Task<Product> UpdateProduct(UpdateProductDTO model);

        /// <summary>
        /// Elimina un prodotto
        /// </summary>
        /// <param name="id">ID del prodotto da eliminare</param>
        /// <returns>True se l'eliminazione ha successo, altrimenti false</returns>
        Task<bool> DeleteProduct(int id);
    }
}
