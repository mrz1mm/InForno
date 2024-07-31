using InForno.Models;
using InForno.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace InForno.Controllers
{
    public class CustomerController : Controller
    {
        private readonly InFornoDbContext _context;
        public CustomerController(InFornoDbContext context)
        {
            _context = context;
        }

        // VISTE - Orders
        public async Task<IActionResult> Orders()
        {
            var orders = await _context.Orders.ToListAsync();
            return View();
        }


        // METODI - Cart
        private List<Cart> GetCartFromSession()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<Cart>();
            }
            return JsonConvert.DeserializeObject<List<Cart>>(cartJson);
        }

        private void SaveCartToSession(List<Cart> cart)
        {
            var cartJson = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("Cart", cartJson);
        }

        public async Task<IActionResult> AddProductsToCart(int ProductId, int quantity, string returnUrl)
        {
            var cart = GetCartFromSession();
            var product = await _context.Products.FindAsync(ProductId); ;

            if (product == null)
            {
                return NotFound();
            }

            var existingProduct = cart.FirstOrDefault(x => x.Product.ProductId == ProductId);
            if (existingProduct != null)
            {
                existingProduct.Quantity += quantity;
            }
            else
            {
                cart.Add(new Cart
                {
                    Product = product,
                    Quantity = quantity
                });
            }

            SaveCartToSession(cart);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> RemoveProductFromCart(int ProductId, string returnUrl)
        {
            var cart = await Task.Run(() => GetCartFromSession());
            var productToRemove = cart.FirstOrDefault(x => x.Product.ProductId == ProductId);
            if (productToRemove != null)
            {
                productToRemove.Quantity -= 1;
                if (productToRemove.Quantity <= 0)
                {
                    cart.Remove(productToRemove);
                }
            }

            await Task.Run(() => SaveCartToSession(cart));

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ClearCart(string returnUrl)
        {
            HttpContext.Session.Remove("Cart");

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }


        // METODI - Orders
        public async Task<IActionResult> CreateOrder(string returnUrl, string note, string address)
        {
            var cart = GetCartFromSession();
            if (cart == null || !cart.Any())
            {
                return BadRequest("Il carrello è vuoto.");
            }

            // Ottieni l'ID dell'utente corrente
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("Utente non autenticato.");
            }

            // Ottieni l'utente dal database
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId.ToString() == userId);
            if (user == null)
            {
                return Unauthorized("Utente non trovato.");
            }

            // Crea un nuovo ordine
            var order = new Order
            {
                CartItems = cart,
                User = user,
                Address = address,
                Note = note,
                DateTime = DateTime.Now
            };

            // Aggiungi l'ordine al contesto e salva le modifiche
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Svuota il carrello
            HttpContext.Session.Remove("Cart");

            // Reindirizza l'utente
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
