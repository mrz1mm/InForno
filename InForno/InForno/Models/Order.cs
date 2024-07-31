using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InForno.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public List<Cart> CartItems { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public string Address { get; set; }

        public string Note { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}
