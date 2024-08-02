using InForno.Models;
using InForno.Models.DTO;
using InForno.Models.VM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        [HttpGet]
        public IActionResult Cart()
        {
            var cart = _cartSvc.GetCart();
            return View(cart);
        }

        // CART - Metodi
        [HttpPost]
        public async Task<IActionResult> AddProductsToCart(CartDTO cartDTO, string returnUrl)
        {
            try
            {
                await _cartSvc.AddProductsToCart(cartDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Catalog", "Home");
        }

        public async Task<IActionResult> RemoveProductFromCart(int ProductId, string returnUrl)
        {
            await _cartSvc.RemoveProductFromCart(ProductId);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Cart", "Customer");
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
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            var orders = await _orderSvc.GetOrdersByCurrentUser();
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> CheckOrder()
        {
            var cartDTOs = _cartSvc.GetCartFromSession();
            if (cartDTOs == null || !cartDTOs.Any())
            {
                ModelState.AddModelError(string.Empty, "Il carrello è vuoto.");
                return RedirectToAction("Cart");
            }

            var cartVM = new List<CheckOrderVM>();
            foreach (var cartDTO in cartDTOs)
            {
                var product = await _context.Products.FindAsync(cartDTO.ProductId);
                if (product == null)
                {
                    ModelState.AddModelError(string.Empty, "Prodotto non trovato.");
                    return RedirectToAction("Cart");
                }

                cartVM.Add(new CheckOrderVM
                {
                    ProductName = product.Name,
                    ProductPrice = product.Price,
                    Quantity = cartDTO.Quantity
                });
            }

            return View(cartVM);
        }

        public async Task<IActionResult> OrderConfirmed()
        {
            return await Task.FromResult(View());
        }

        // METODI - Orders
        [HttpGet]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _orderSvc.GetOrderById(orderId);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(string Address, string Note)
        {
            var cartDTOs = _cartSvc.GetCartFromSession();
            if (cartDTOs == null || !cartDTOs.Any())
            {
                ModelState.AddModelError(string.Empty, "Il carrello è vuoto.");
                return RedirectToAction("CheckOrder");
            }

            var cartItems = new List<Cart>();
            foreach (var cartDTO in cartDTOs)
            {
                var product = await _context.Products.FindAsync(cartDTO.ProductId);
                if (product == null)
                {
                    ModelState.AddModelError(string.Empty, "Prodotto non trovato.");
                    return RedirectToAction("CheckOrder");
                }

                cartItems.Add(new Cart
                {
                    Product = product,
                    Quantity = cartDTO.Quantity
                });
            }

            var orderDTO = new OrderDTO
            {
                CartItems = cartItems,
                Address = Address,
                Note = Note
            };

            if (!ModelState.IsValid)
            {
                return View("CheckOrder", cartItems);
            }

            var success = await _orderSvc.CreateOrder(orderDTO);

            if (!success)
            {
                return BadRequest("Errore nella creazione dell'ordine.");
            }

            _cartSvc.ClearCart();
            return RedirectToAction("OrderConfirmed");
        }
    }
}
