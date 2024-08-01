using InForno.Models;
using InForno.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

public class CartSvc
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly InFornoDbContext _context;

    public CartSvc(IHttpContextAccessor httpContextAccessor, InFornoDbContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }

    private List<Cart> GetCartFromSession()
    {
        var cartJson = _httpContextAccessor.HttpContext.Session.GetString("Cart");
        if (string.IsNullOrEmpty(cartJson))
        {
            return new List<Cart>();
        }
        return JsonConvert.DeserializeObject<List<Cart>>(cartJson);
    }

    public List<Cart> GetCart()
    {
        return GetCartFromSession();
    }

    private void SaveCartToSession(List<Cart> cart)
    {
        var cartJson = JsonConvert.SerializeObject(cart);
        _httpContextAccessor.HttpContext.Session.SetString("Cart", cartJson);
    }

    public async Task AddProductsToCart(CartDTO cartDTO)
    {
        var cart = GetCartFromSession();
        var existingProduct = cart.FirstOrDefault(x => x.Product.ProductId == cartDTO.ProductId);
        if (existingProduct != null)
        {
            existingProduct.Quantity += cartDTO.Quantity;
        }
        else
        {
            var product = await _context.Products.FindAsync(cartDTO.ProductId);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            cart.Add(new Cart
            {
                Product = product,
                Quantity = cartDTO.Quantity
            });
        }
        SaveCartToSession(cart);
    }

    public async Task RemoveProductFromCart(int productId)
    {
        var cart = GetCartFromSession();
        var productToRemove = cart.FirstOrDefault(x => x.Product.ProductId == productId);
        if (productToRemove != null)
        {
            productToRemove.Quantity -= 1;
            if (productToRemove.Quantity <= 0)
            {
                cart.Remove(productToRemove);
            }
        }
        SaveCartToSession(cart);
    }

    public void ClearCart()
    {
        _httpContextAccessor.HttpContext.Session.Remove("Cart");
    }
}
