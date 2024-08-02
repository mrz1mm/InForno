using InForno.Models;
using InForno.Models.DTO;
using InForno.Models.VM;
using InForno.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InForno.Controllers
{
    [Authorize(Policy = Policies.Customer)]
    public class CustomerController : Controller
    {
        private readonly InFornoDbContext _context;
        private readonly ICartSvc _cartSvc;
        private readonly IOrderSvc _orderSvc;

        public CustomerController(InFornoDbContext context, ICartSvc cartSvc, IOrderSvc orderSvc)
        {
            _context = context;
            _cartSvc = cartSvc;
            _orderSvc = orderSvc;
        }

        // CART - Views
        [HttpGet]
        public async Task<IActionResult> Cart()
        {
            var cartDTOs = _cartSvc.GetCartFromSession();
            if (cartDTOs == null || !cartDTOs.Any())
            {
                return View(new List<CartVM>());
            }

            var cartVMs = new List<CartVM>();
            foreach (var cartDTO in cartDTOs)
            {
                var product = await _context.Products.FindAsync(cartDTO.ProductId);
                if (product == null)
                {
                    ModelState.AddModelError(string.Empty, "Prodotto non trovato.");
                    return View(new List<CartVM>());
                }

                cartVMs.Add(new CartVM
                {
                    ProductId = product.ProductId,
                    ProductName = product.Name,
                    ProductPrice = product.Price,
                    Quantity = cartDTO.Quantity
                });
            }

            return View(cartVMs);
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

        [Authorize(Policy = Policies.SupplierOrCustomer)]
        [HttpGet]
        public async Task<IActionResult> OrderDetails(int orderId)
        {
            var order = await _orderSvc.GetOrderById(orderId);
            if (order == null)
            {
                return NotFound();
            }

            var orderDetailsVM = new OrderDetailsVM
            {
                OrderId = order.OrderId,
                OrderDate = order.DateTime,
                Address = order.Address,
                Note = order.Note,
                Items = order.CartItems.Select(ci => new OrderItemVM
                {
                    ProductName = ci.Product.Name,
                    ProductPrice = ci.Product.Price,
                    Quantity = ci.Quantity
                }).ToList()
            };

            return View(orderDetailsVM);
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
        public async Task<IActionResult> AddOrder(PreAddOrderDTO preAddOrderDTO)
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

            if (!ModelState.IsValid)
            {
                return View("CheckOrder", cartItems);
            }

            var orderDTO = new AddOrderDTO
            {
                CartItems = cartItems,
                Address = preAddOrderDTO.Address,
                Note = preAddOrderDTO.Note
            };


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
