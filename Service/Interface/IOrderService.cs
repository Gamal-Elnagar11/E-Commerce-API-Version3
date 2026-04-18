using E_Commerce_API.Models;
using E_Commerce_API.Static;

namespace E_Commerce_API.Service.Interface
{
    public interface IOrderService
    {
         Task<Order> Checkout(string userId, string phone, string city, string address, Payment paymentMethod, CancellationToken ct = default);

         Task<List<Order>> GetOrdersByUser(string userId, CancellationToken ct = default);

         Task<Order> GetOrderById(int orderId, CancellationToken ct = default);

         Task<List<Order>> GetAllOrders(CancellationToken ct = default);

         Task<Order> UpdateOrderStatus(int orderId, OrderStatus status, CancellationToken ct = default);
    }
}

