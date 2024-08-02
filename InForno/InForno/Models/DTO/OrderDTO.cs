using System.ComponentModel.DataAnnotations;

namespace InForno.Models.DTO
{
    public class OrderDTO
    {
        [Required]
        public List<Cart> CartItems { get; set; }

        [Required]
        public string Address { get; set; }

        public string Note { get; set; }
    }
}
