using InForno.Models;
using InForno.Models.DTO;

namespace InForno.Services
{
    /// <summary>
    /// Interfaccia per il servizio degli ordini
    /// </summary>
    public interface IOrderSvc
    {
        /// <summary>
        /// Ottiene tutti gli ordini
        /// </summary>
        /// <returns>Lista di Order</returns>
        Task<List<Order>> GetOrders();

        /// <summary>
        /// Ottiene un ordine per ID
        /// </summary>
        /// <param name="orderId">ID dell'ordine</param>
        /// <returns>Ordine</returns>
        Task<Order> GetOrderById(int orderId);

        /// <summary>
        /// Ottiene gli ordini dell'utente corrente
        /// </summary>
        /// <returns>Lista di Order</returns>
        Task<List<Order>> GetOrdersByCurrentUser();

        /// <summary>
        /// Crea un nuovo ordine
        /// </summary>
        /// <param name="orderDTO">Dati dell'ordine</param>
        /// <returns>True se la creazione ha successo, altrimenti false</returns>
        Task<bool> CreateOrder(AddOrderDTO orderDTO);

        /// <summary>
        /// Alterna lo stato di pagamento di un ordine
        /// </summary>
        /// <param name="orderId">ID dell'ordine</param>
        /// <returns>Task completato</returns>
        Task ToggleIsPaid(int orderId);
    }
}
