using Ordernary.Models;

namespace Ordernary.Repositories.Interface
{
    public interface IOrderInterface
    {
        Task<IEnumerable<Order>> GetAllOrders();
        Task<Order> GetOrderById(int id);
        Task CreateOrder(Order order);
        Task DeleteOrder(int id);
    }
}
