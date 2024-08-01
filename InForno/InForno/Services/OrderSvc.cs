using InForno.Models;
using InForno.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class OrderSvc
{
    private readonly InFornoDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AuthSvc _authSvc;

    public OrderSvc(InFornoDbContext context, IHttpContextAccessor httpContextAccessor, AuthSvc authSvc)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _authSvc = authSvc;
    }

    public async Task<List<Order>> GetOrders()
    {
        return await _context.Orders
            .Include(o => o.CartItems)
            .ThenInclude(ci => ci.Product)
            .Include(o => o.User)
            .ToListAsync();
    }

    public async Task<Order> GetOrderById(int orderId)
    {
        return await _context.Orders
            .Include(o => o.CartItems)
            .ThenInclude(ci => ci.Product)
            .Include(o => o.User)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);
    }

    public async Task<List<Order>> GetOrdersByCurrentUser()
    {
        var userId = _authSvc.GetCurrentUserId();
        if (userId == null)
        {
            return new List<Order>();
        }

        return await _context.Orders
            .Include(o => o.User)
            .Where(o => o.User.UserId.ToString() == userId)
            .ToListAsync();
    }

    public async Task<bool> CreateOrder(List<Cart> cart, string note, string address)
    {
        if (cart == null || !cart.Any())
        {
            return false;
        }

        var userId = _authSvc.GetCurrentUserId();
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
