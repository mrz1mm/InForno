using InForno.Models;
using Newtonsoft.Json;

public class CartSvc
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CartSvc(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
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

    public async Task AddProductsToCart(Product product, int quantity)
    {
        var cart = GetCartFromSession();
        var existingProduct = cart.FirstOrDefault(x => x.Product.ProductId == product.ProductId);
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
