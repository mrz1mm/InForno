using InForno.Models;
using InForno.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InForno.Controllers
{
    public class SupplierController : Controller
    {
        private readonly InFornoDbContext _context;
        public SupplierController(InFornoDbContext context)
        {
            _context = context;
        }


        // VISTE - Orders
        public async Task<IActionResult> Orders()
        {
             return View();
        }


        // VISTE - Products
        public async Task<IActionResult> Products()
        {
            var products = await _context.Products.Include(c => c.Ingredients).ToListAsync();
            return View();
        }

        public IActionResult AddProduct()
        {
            return View();
        }

        public IActionResult UpdateProduct()
        {
            return View();
        }

        public IActionResult DeleteProduct()
        {
            return View();
        }


        // VISTE - Ingredients
        public async Task<IActionResult> Ingredients()
        {
            var ingredients = await _context.Ingredients.ToListAsync();
            return View();
        }

        public IActionResult AddIngredient()
        {
            return View();
        }

        public IActionResult UpdateIngredient() {
            return View();
        }

        public IActionResult DeleteIngredient()
        {
            return View();
        }


        // METODI - Orders


        // METODI - Products
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct([Bind("Name, Price, Description, DeliveryTime, ProductImage, Ingredients")] ProductDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                Description = model.Description,
                DeliveryTime = model.DeliveryTime,
                ProductImage = model.ProductImage,
                Ingredients = model.Ingredients
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Products");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProduct([Bind("ProductId, Name, Price, Description, DeliveryTime, ProductImage, Ingredients")] Product model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                Description = model.Description,
                DeliveryTime = model.DeliveryTime,
                ProductImage = model.ProductImage,
                Ingredients = model.Ingredients
            };

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Products");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct([Bind("ProductId")] Product model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var product = new Product
            {
                ProductId = model.ProductId
            };

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Products");
        }


        // METODI - Ingredients
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddIngredient([Bind("Name, Description, IngredientImage")] Ingredient model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var ingredient = new Ingredient
            {
                Name = model.Name,
                Description = model.Description,
            };

            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();
            return RedirectToAction("Ingredients");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateIngredient([Bind("IngredientId, Name, Description, IngredientImage")] Ingredient model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var ingredient = new Ingredient
            {
                Name = model.Name,
                Description = model.Description,
            };

            _context.Ingredients.Update(ingredient);
            await _context.SaveChangesAsync();
            return RedirectToAction("Ingredients");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteIngredient([Bind("IngredientId")] Ingredient model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var ingredient = new Ingredient
            {
                IngredientId = model.IngredientId
            };

            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();
            return RedirectToAction("Ingredients");
        }
    }
}
