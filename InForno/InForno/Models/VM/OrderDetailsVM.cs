namespace InForno.Models.VM
{
    public class OrderDetailsVM
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public List<OrderItemVM> Items { get; set; }
    }

    public class OrderItemVM
    {
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
    }
}