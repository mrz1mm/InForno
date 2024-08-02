using InForno.Models;
using InForno.Models.DTO;
using InForno.Services;
using Microsoft.EntityFrameworkCore;
public class OrderSvc : IOrderSvc
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

    public async Task<bool> CreateOrder(AddOrderDTO orderDTO)
    {
        if (orderDTO.CartItems == null || !orderDTO.CartItems.Any())
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
            CartItems = new List<Cart>(),
            User = user,
            Address = orderDTO.Address,
            Note = orderDTO.Note,
            DateTime = DateTime.Now
        };

        foreach (var item in orderDTO.CartItems)
        {
            var cartItem = new Cart
            {
                Product = item.Product,
                Quantity = item.Quantity
            };
            order.CartItems.Add(cartItem);
            _context.Carts.Add(cartItem);
        }

        _context.Orders.Add(order);

        var success = await _context.SaveChangesAsync() > 0;

        return success;
    }

    public async Task ToggleIsPaid(int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
        {
            throw new Exception("Ordine non trovato");
        }
        order.IsPaid = !order.IsPaid;
        await _context.SaveChangesAsync();
    }
}
