using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InForno.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }        
    }
}
