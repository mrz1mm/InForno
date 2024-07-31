using InForno.Models;
using InForno.Models.DTO;
using InForno.Svc;
using Microsoft.EntityFrameworkCore;

namespace InForno.Services
{
    public class ProductSvc
    {
        private readonly InFornoDbContext _context;
        private readonly ImageSvc _imageSvc;

        public ProductSvc(InFornoDbContext context, ImageSvc imageSvc)
        {
            _context = context;
            _imageSvc = imageSvc;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.Include(p => p.Ingredients).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.Include(p => p.Ingredients).FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<Product> AddProductAsync(AddProductDTO model)
        {
            var imageUrl = await _imageSvc.SaveImageAsync(model.ProductImageFile);

            var product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                Description = model.Description,
                DeliveryTime = model.DeliveryTime,
                ProductImageUrl = imageUrl,
                Ingredients = model.Ingredients.Select(i => new Ingredient { IngredientId = i }).ToList()
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
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
