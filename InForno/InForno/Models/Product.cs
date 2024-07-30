using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InForno.Models
{
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        public List<Image> Images { get; set; }

        [Required]
        public List<Ingredient> Ingredients { get; set; }
    }
}
