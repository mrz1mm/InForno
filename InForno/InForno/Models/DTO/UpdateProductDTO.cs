namespace InForno.Models.DTO
{
    public class UpdateProductDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public TimeSpan DeliveryTime { get; set; }
        public IFormFile ProductImageFile { get; set; }
        public string ProductImageUrl { get; set; }
        public List<int> Ingredients { get; set; }
    }

    public class IngredientCheckbox
    {
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }

    public class UpdateProductViewModel
    {
        public UpdateProductDTO Product { get; set; }
        public List<IngredientCheckbox> Ingredients { get; set; }
    }

}
