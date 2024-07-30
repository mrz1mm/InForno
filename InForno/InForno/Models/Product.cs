namespace InForno.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string DeliveryTime { get; set; }
        public List<Image> Images { get; set; }
        public List<Ingredient> Ingredients { get; set; }
    }
}
