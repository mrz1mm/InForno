﻿namespace InForno.Models.DTO
{
    public class UpdateProductDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public TimeSpan DeliveryTime { get; set; }
        public IFormFile ProductImageFile { get; set; }
        public List<int> Ingredients { get; set; }
    }
}
