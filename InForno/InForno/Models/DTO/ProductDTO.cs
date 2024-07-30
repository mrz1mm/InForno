namespace InForno.Models.DTO
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string DeliveryTime { get; set; }
        public byte[] ProductImage { get; set; }
        public List<Ingredient> Ingredients { get; set; }
    }
}
