using System.ComponentModel.DataAnnotations;

namespace InForno.Models.DTO
{
    public class PreAddOrderDTO
    {
        public string Address { get; set; }
        public string Note { get; set; } = "N/D";
    }


    public class AddOrderDTO
    {
        [Required]
        public List<Cart> CartItems { get; set; }

        [Required]
        public string Address { get; set; }

        public string Note { get; set; }
    }
}
