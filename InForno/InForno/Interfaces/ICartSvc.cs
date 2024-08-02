using InForno.Models.DTO;

namespace InForno.Services
{
    /// <summary>
    /// Interfaccia per il servizio del carrello
    /// </summary>
    public interface ICartSvc
    {
        /// <summary>
        /// Ottiene il carrello dalla sessione
        /// </summary>
        /// <returns>Lista di CartDTO</returns>
        List<CartDTO> GetCartFromSession();

        /// <summary>
        /// Ottiene il carrello
        /// </summary>
        /// <returns>Lista di CartDTO</returns>
        List<CartDTO> GetCart();

        /// <summary>
        /// Aggiunge prodotti al carrello
        /// </summary>
        /// <param name="cartDTO">Dati del prodotto da aggiungere</param>
        /// <returns>Task completato</returns>
        Task AddProductsToCart(CartDTO cartDTO);

        /// <summary>
        /// Rimuove un prodotto dal carrello
        /// </summary>
        /// <param name="productId">ID del prodotto da rimuovere</param>
        /// <returns>Task completato</returns>
        Task RemoveProductFromCart(int productId);

        /// <summary>
        /// Svuota il carrello
        /// </summary>
        void ClearCart();
    }
}
