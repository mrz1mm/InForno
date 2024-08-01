using InForno.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class OrderSvc
{
    private readonly InFornoDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public OrderSvc(InFornoDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }


    public async Task<Order> GetOrderById(int orderId)
    {
        return await _context.Orders
            .Include(o => o.CartItems)
            .ThenInclude(ci => ci.Product)
            .Include(o => o.User)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);
    }

    public async Task<List<Order>> GetOrdersBySupplier(int supplierId)
    {
        return await _context.Orders
            .Include(o => o.CartItems)
            .ThenInclude(ci => ci.Product)
            .Where(o => o.CartItems.Any(ci => ci.Product.SupplierId == supplierId))
            .ToListAsync();
    }

    public async Task<List<Order>> GetOrdersByCustomer(int customerId)
    {
        return await _context.Orders
            .Include(o => o.User)
            .Where(o => o.User.UserId == customerId)
            .ToListAsync();
    }

    public async Task<bool> CreateOrder(List<Cart> cart, string note, string address)
    {
        if (cart == null || !cart.Any())
        {
            return false;
        }

        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return false;
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId.ToString() == userId);
        if (user == null)
        {
            return false;
        }

        var order = new Order
        {
            CartItems = cart,
            User = user,
            Address = address,
            Note = note,
            DateTime = DateTime.Now
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return true;
    }
}
