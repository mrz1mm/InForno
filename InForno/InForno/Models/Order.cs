namespace InForno.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public User User { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }        
    }
}
