﻿namespace InForno.Models
{
    public class Checkout
    {
        public int CheckoutId { get; set; }
        public List<Product> Products { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public DateTime DateTime { get; set; }
    }
}
