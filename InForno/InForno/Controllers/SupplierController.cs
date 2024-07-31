using InForno.Models;
using InForno.Models.DTO;
using InForno.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InForno.Controllers
{
    public class SupplierController : Controller
    {
        private readonly InFornoDbContext _context;
        private readonly ProductSvc _productSvc;
        private readonly IngredientSvc _ingredientSvc;

        public SupplierController(InFornoDbContext context, ProductSvc productSvc, IngredientSvc ingredientSvc)
        {
            _context = context;
            _productSvc = productSvc;
            _ingredientSvc = ingredientSvc;
        }


        // VISTE - Orders
        public async Task<IActionResult> Orders()
        {
            return View();
        }


        // VISTE - Products
        public async Task<IActionResult> Products()
        {
            var products = await _productSvc.GetAllProductsAsync();
            return View(products);
        }

        public async Task<IActionResult> AddProduct()
        {
            var ingredients = await _ingredientSvc.GetAllIngredientsAsync();
            ViewBag.Ingredients = new SelectList(ingredients, "IngredientId", "Name");

            var model = new AddProductDTO();
            return View(model);
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
        [HttpGet]
        public async Task<IActionResult> Ingredients()
        {
            var ingredients = await _ingredientSvc.GetAllIngredientsAsync();
            return View(ingredients);
        }

        public IActionResult AddIngredient()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateIngredient(int id)
        {
            var ingredient = await _ingredientSvc.GetIngredientByIdAsync(id);
            if (ingredient == null)
            {
                return NotFound();
            }

            var model = new UpdateIngredientDTO
            {
                IngredientId = ingredient.IngredientId,
                Name = ingredient.Name
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteIngredient(int id)
        {
            var ingredient = await _ingredientSvc.GetIngredientByIdAsync(id);
            if (ingredient == null)
            {
                return NotFound();
            }

            var model = new UpdateIngredientDTO
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
        public async Task<IActionResult> AddProduct([Bind("Name, Price, Description, DeliveryTime, ProductImageFile, Ingredients")] AddProductDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _productSvc.AddProductAsync(model);
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
                await _productSvc.UpdateProductAsync(model);
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
                var success = await _productSvc.DeleteProductAsync(model.ProductId);
                if (!success)
                {
                    return NotFound();
                }
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
                await _ingredientSvc.AddIngredientAsync(model);
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
        public async Task<IActionResult> UpdateIngredient([Bind("IngredientId, Name")] UpdateIngredientDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _ingredientSvc.UpdateIngredientAsync(model);
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
        public async Task<IActionResult> ConfirmDeleteIngredient(int ingredientId)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                var success = await _ingredientSvc.DeleteIngredientAsync(ingredientId);
                if (!success)
                {
                    return NotFound();
                }
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
