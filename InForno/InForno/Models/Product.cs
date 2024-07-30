using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace InForno.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Precision(2)]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string DeliveryTime { get; set; }

        [Required]
        public List<Ingredient> Ingredients { get; set; }

        [Required]
        public byte[] ProductImage { get; set; }

        public string ProductImageUrl => ProductImage != null ? $"data:image/jpeg;base64,{Convert.ToBase64String(ProductImage)}" : null;
    }
}
