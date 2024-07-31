﻿using InForno.Models;
using InForno.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace InForno.Services
{
    public class ProductSvc
    {
        private readonly InFornoDbContext _context;
        private readonly ImageSvc _imageSvc;
        private readonly IngredientSvc _ingredientSvc;

        public ProductSvc(InFornoDbContext context, ImageSvc imageSvc, IngredientSvc ingredientSvc)
        {
            _context = context;
            _imageSvc = imageSvc;
            _ingredientSvc = ingredientSvc;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.Include(p => p.Ingredients).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.Include(p => p.Ingredients).FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task AddProductAsync(AddProductDTO model)
        {
            var product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                Description = model.Description,
                DeliveryTime = model.DeliveryTime,
                ProductImageUrl = model.ProductImageFile != null ? await _imageSvc.SaveImageAsync(model.ProductImageFile) : null,
                Ingredients = new List<Ingredient>()
            };

            if (model.Ingredients != null && model.Ingredients.Any())
            {
                var selectedIngredients = await _ingredientSvc.GetIngredientsByIdsAsync(model.Ingredients);
                product.Ingredients.AddRange(selectedIngredients);
            }

            if (product.DeliveryTime < TimeSpan.Zero || product.DeliveryTime >= TimeSpan.FromHours(24))
            {
                throw new ArgumentException("Il valore di DeliveryTime è fuori dal range accettabile.");
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
