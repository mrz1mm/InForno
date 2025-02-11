﻿using Microsoft.EntityFrameworkCore;
namespace InForno.Models
{
    public class InFornoDbContext : DbContext
    {
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Ingredient> Ingredients { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public InFornoDbContext(DbContextOptions options) : base(options) { }
    }
}
