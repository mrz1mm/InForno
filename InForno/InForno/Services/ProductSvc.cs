using InForno.Models;
using InForno.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace InForno.Services
{
    public class ProductSvc : IProductSvc
    {
        private readonly InFornoDbContext _context;
        private readonly IImageSvc _imageSvc;
        private readonly IIngredientSvc _ingredientSvc;

        public ProductSvc(InFornoDbContext context, IImageSvc imageSvc, IIngredientSvc ingredientSvc)
        {
            _context = context;
            _imageSvc = imageSvc;
            _ingredientSvc = ingredientSvc;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.Products.Include(p => p.Ingredients).ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products.Include(p => p.Ingredients).FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task AddProduct(AddProductDTO model)
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
                var selectedIngredients = await _ingredientSvc.GetIngredientsByIds(model.Ingredients);
                product.Ingredients.AddRange(selectedIngredients);
            }

            if (product.DeliveryTime < TimeSpan.Zero || product.DeliveryTime >= TimeSpan.FromHours(24))
            {
                throw new ArgumentException("Il valore di DeliveryTime è fuori dal range accettabile.");
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> UpdateProduct(UpdateProductDTO model)
        {
            var product = await _context.Products.Include(p => p.Ingredients).FirstOrDefaultAsync(p => p.ProductId == model.ProductId);
            if (product == null)
            {
                throw new ArgumentException("Prodotto non trovato.");
            }

            product.Name = model.Name;
            product.Price = model.Price;
            product.Description = model.Description;
            product.DeliveryTime = model.DeliveryTime;

            if (model.ProductImageFile != null)
            {
                if (!string.IsNullOrEmpty(product.ProductImageUrl))
                {
                    await _imageSvc.DeleteImageAsync(product.ProductImageUrl);
                }
                product.ProductImageUrl = await _imageSvc.SaveImageAsync(model.ProductImageFile);
            }

            product.Ingredients.Clear();
            if (model.Ingredients != null && model.Ingredients.Any())
            {
                var selectedIngredients = await _ingredientSvc.GetIngredientsByIds(model.Ingredients);
                product.Ingredients.AddRange(selectedIngredients);
            }

            if (product.DeliveryTime < TimeSpan.Zero || product.DeliveryTime >= TimeSpan.FromHours(24))
            {
                throw new ArgumentException("Il valore di DeliveryTime è fuori dal range accettabile.");
            }

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProduct(int id)
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
