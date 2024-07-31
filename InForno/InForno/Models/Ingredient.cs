using System.ComponentModel.DataAnnotations;

namespace InForno.Models
{
    public class Ingredient
    {
            [Key]
            public int IngredientId { get; set; }

            [Required]
            public string Name { get; set; }

            public List<Product> Products { get; set; }
    }
}
