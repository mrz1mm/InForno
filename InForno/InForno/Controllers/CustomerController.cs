using InForno.Models;
using InForno.Models.DTO;
using InForno.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InForno.Controllers
{
    public class CustomerController : Controller
    {
        private readonly InFornoDbContext _context;
        private readonly CartSvc _cartSvc;
        private readonly OrderSvc _orderSvc;

        public CustomerController(InFornoDbContext context, CartSvc cartSvc, OrderSvc orderSvc)
        {
            _context = context;
            _cartSvc = cartSvc;
            _orderSvc = orderSvc;
        }


        // CART - Views
        public IActionResult Cart()
        {
            var cart = _cartSvc.GetCart();
            return View(cart);
        }


        // CART - Metodi
        public async Task<IActionResult> AddProductsToCart(int ProductId, int quantity, string returnUrl)
        {
            var product = await _context.Products.FindAsync(ProductId);
            if (product == null)
            {
                return NotFound();
            }

            await _cartSvc.AddProductsToCart(product, quantity);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> RemoveProductFromCart(int ProductId, string returnUrl)
        {
            await _cartSvc.RemoveProductFromCart(ProductId);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult ClearCart(string returnUrl)
        {
            _cartSvc.ClearCart();

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }





        // ORDERS - Views
        public async Task<IActionResult> Orders()
        {
            var orders = await _context.Orders.ToListAsync();
            return View(orders);
        }


        // METODI - Orders
        public async Task<IActionResult> CreateOrder(string returnUrl, string note, string address)
        {
            var cart = _cartSvc.GetCart();
            var success = await _orderSvc.CreateOrder(cart, note, address);

            if (!success)
            {
                return BadRequest("Errore nella creazione dell'ordine.");
            }

            _cartSvc.ClearCart();

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> GetOrdersByCustomer(int customerId)
        {
            var orders = await _orderSvc.GetOrdersByCustomer(customerId);
            return View(orders);
        }

    }
}
