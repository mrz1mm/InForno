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
        public TimeSpan DeliveryTime { get; set; }

        [Required]
        public string ProductImageUrl { get; set; }

        [Required]
        public List<Ingredient> Ingredients { get; set; }
    }
}
