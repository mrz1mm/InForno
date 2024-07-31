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
        public async Task<IActionResult> OrdersView()
        {
             return View();
        }


        // VISTE - Products
        public async Task<IActionResult> ProductsView()
        {
            var products = await _context.Products.Include(c => c.Ingredients).ToListAsync();
            return View(products);
        }

        public IActionResult AddProductView()
        {
            return View();
        }

        public IActionResult UpdateProductView()
        {
            return View();
        }

        public IActionResult DeleteProductView()
        {
            return View();
        }


        // VISTE - Ingredients
        [HttpGet]
        public async Task<IActionResult> IngredientsView()
        {
            var ingredients = await _context.Ingredients.ToListAsync();
            return View(ingredients);
        }

        public IActionResult AddIngredientView()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateIngredientView(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null)
            {
                return NotFound();
            }

            var model = new IngredientDTO
            {
                IngredientId = ingredient.IngredientId,
                Name = ingredient.Name
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteIngredientView(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null)
            {
                return NotFound();
            }

            var model = new IngredientDTO
            {
                IngredientId = ingredient.IngredientId,
                Name = ingredient.Name
            };

            return View(model);
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

            try
            {
                var product = new Product
                {
                    Name = model.Name,
                    Price = model.Price,
                    Description = model.Description,
                    DeliveryTime = model.DeliveryTime,
                    ProductImage = model.ProductImage,
                    Ingredients = new List<Ingredient>()
                };

                // Associa gli ingredienti al prodotto
                if (model.Ingredients != null && model.Ingredients.Any())
                {
                    product.Ingredients = new List<Ingredient>();
                    foreach (var ingredient in model.Ingredients)
                    {
                        var existingIngredient = await _context.Ingredients.FindAsync(ingredient.IngredientId);
                        if (existingIngredient != null)
                        {
                            product.Ingredients.Add(existingIngredient);
                        }
                    }
                }

                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Products");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Si è verificato un errore durante l'aggiunta del prodotto.");
                return View(model);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProduct([Bind("ProductId, Name, Price, Description, DeliveryTime, ProductImage, Ingredients")] Product model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
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
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Si è verificato un errore durante l'aggiornamento del prodotto.");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct([Bind("ProductId")] Product model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var product = new Product
                {
                    ProductId = model.ProductId
                };

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Products");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Si è verificato un errore durante l'eliminazione del prodotto.");
                return View(model);
            }
        }



        // METODI - Ingredients
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddIngredient([Bind("Name")] AddIngredientDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var ingredient = new Ingredient
                {
                    Name = model.Name,
                };

                _context.Ingredients.Add(ingredient);
                await _context.SaveChangesAsync();
                return RedirectToAction("Ingredients");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Si è verificato un errore durante l'aggiunta dell'ingrediente.");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateIngredient([Bind("IngredientId, Name")] IngredientDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var ingredient = new Ingredient
                {
                    IngredientId = model.IngredientId,
                    Name = model.Name,
                };

                _context.Ingredients.Update(ingredient);
                await _context.SaveChangesAsync();
                return RedirectToAction("Ingredients");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Si è verificato un errore durante l'aggiornamento dell'ingrediente.");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteIngredient(int ingredientId)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                var ingredient = await _context.Ingredients.FindAsync(ingredientId);
                if (ingredient == null)
                {
                    return NotFound();
                }

                _context.Ingredients.Remove(ingredient);
                await _context.SaveChangesAsync();
                return RedirectToAction("Ingredients");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Si è verificato un errore durante l'eliminazione dell'ingrediente.");
                return View();
            }
        }
    }
}
