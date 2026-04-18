using E_Commerce_API.Models;
using E_Commerce_API.Static;

namespace E_Commerce_API.Reposatory.Interface
{
    public interface IOrderRepo
    { 
             Task AddOrder(Order order, CancellationToken ct = default);

             Task<Order> GetOrderById(int orderId, CancellationToken ct = default);

             Task<List<Order>> GetOrdersByUserId(string userId, CancellationToken ct = default);

             Task<List<Order>> GetAllOrders(CancellationToken ct = default);

             Task UpdateOrderStatus(int orderId, OrderStatus status, CancellationToken ct = default);

            
        
    }
}
