using Microsoft.EntityFrameworkCore;
using Ordernary.Data;
using Ordernary.Models;
using Ordernary.Repositories.Interface;

namespace Ordernary.Repositories.Implementation
{
    public class OrderRepository : IOrderInterface
    {
        private readonly Context _context;

        public OrderRepository(Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _context.Orders
                .Include(o => o.OrderArticles)
                .ThenInclude(oa => oa.Article)
                .ToListAsync();
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderArticles)
                .ThenInclude(oa => oa.Article)
                .FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task CreateOrder(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}
