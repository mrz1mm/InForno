using InForno.Models;
using InForno.Models.DTO;
using InForno.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InForno.Controllers
{
    [Authorize(Policy = "Supplier")]
    public class SupplierController : Controller
    {
        private readonly InFornoDbContext _context;
        private readonly IProductSvc _productSvc;
        private readonly IIngredientSvc _ingredientSvc;
        private readonly IOrderSvc _orderSvc;

        public SupplierController(InFornoDbContext context, IProductSvc productSvc, IIngredientSvc ingredientSvc, IOrderSvc orderSvc)
        {
            _context = context;
            _productSvc = productSvc;
            _ingredientSvc = ingredientSvc;
            _orderSvc = orderSvc;
        }


        // ORDERS - Views
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            var orders = await _orderSvc.GetOrders();
            return View(orders);
        }


        // ORDERS - Metodi
        [HttpPost]
        public async Task<IActionResult> ToggleIsPaid(int id)
        {
            try
            {
                var order = await _orderSvc.GetOrderById(id);
                if (order == null)
                {
                    return Json(new { success = false, message = "Ordine non trovato" });
                }

                await _orderSvc.ToggleIsPaid(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }






        // PRODUCTS - Views
        [HttpGet]
        public async Task<IActionResult> Products()
        {
            var products = await _productSvc.GetAllProducts();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> ProductDetail(int id)
        {
            var product = await _productSvc.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            var ingredients = await _ingredientSvc.GetAllIngredients();
            ViewBag.Ingredients = new SelectList(ingredients, "IngredientId", "Name");

            var model = new AddProductDTO();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            var product = await _productSvc.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            var model = new UpdateProductDTO
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                DeliveryTime = product.DeliveryTime,
                Ingredients = product.Ingredients.Select(i => i.IngredientId).ToList()
            };

            var allIngredients = await _ingredientSvc.GetAllIngredients();
            ViewData["Ingredients"] = allIngredients.Select(i => new SelectListItem
            {
                Value = i.IngredientId.ToString(),
                Text = i.Name,
                Selected = model.Ingredients.Contains(i.IngredientId)
            }).ToList();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productSvc.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            var model = new DeleteProductDTO
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                DeliveryTime = product.DeliveryTime,
                ProductImageUrl = product.ProductImageUrl,
                Ingredients = product.Ingredients.Select(i => i.IngredientId).ToList()
            };

            var allIngredients = await _ingredientSvc.GetAllIngredients();
            ViewData["Ingredients"] = allIngredients.Select(i => new SelectListItem
            {
                Value = i.IngredientId.ToString(),
                Text = i.Name,
                Selected = model.Ingredients.Contains(i.IngredientId)
            }).ToList();

            return View(model);
        }


        // PRODUCTS - Metodi
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
                await _productSvc.AddProduct(model);
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
        public async Task<IActionResult> UpdateProduct(UpdateProductDTO model)
        {
            if (!ModelState.IsValid)
            {
                var allIngredients = await _ingredientSvc.GetAllIngredients();
                ViewData["Ingredients"] = allIngredients.Select(i => new SelectListItem
                {
                    Value = i.IngredientId.ToString(),
                    Text = i.Name,
                    Selected = model.Ingredients.Contains(i.IngredientId)
                }).ToList();
                return View(model);
            }

            try
            {
                await _productSvc.UpdateProduct(model);
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
        public async Task<IActionResult> ConfirmDeleteProduct(int productId)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                var success = await _productSvc.DeleteProduct(productId);
                if (!success)
                {
                    return NotFound();
                }
                return RedirectToAction("Products");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Si è verificato un errore durante l'eliminazione del prodotto.");
                return View();
            }
        }





        // INGREDIENTS - Views
        [HttpGet]
        public async Task<IActionResult> Ingredients()
        {
            var ingredients = await _ingredientSvc.GetAllIngredients();
            return View(ingredients);
        }

        public IActionResult AddIngredient()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateIngredient(int id)
        {
            var ingredient = await _ingredientSvc.GetIngredientById(id);
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
            var ingredient = await _ingredientSvc.GetIngredientById(id);
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


        // INGREDIENTS - Metodi
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
                await _ingredientSvc.AddIngredient(model);
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
                await _ingredientSvc.UpdateIngredient(model);
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
                var success = await _ingredientSvc.DeleteIngredient(ingredientId);
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
