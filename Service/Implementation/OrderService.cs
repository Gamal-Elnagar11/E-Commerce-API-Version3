using E_Commerce_API.DTO.OrderDTO;
using E_Commerce_API.Models;
using E_Commerce_API.Reposatory.Interface;
using E_Commerce_API.Service.Interface;
using E_Commerce_API.Static;
using E_Commerce_API.UnitOfWork;

namespace E_Commerce_API.Service.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepo _orderRepo;
        private readonly ICartService _cartService;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IOrderRepo orderRepo, ICartService cartService, IUnitOfWork unitOfWork)
        {
            _orderRepo = orderRepo;
            _cartService = cartService;
            _unitOfWork = unitOfWork;
        }
        public async Task<Order> Checkout(string userId, string phone, string city, string address, Payment paymentMethod, CancellationToken ct = default)
        {
            var trans = await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                var cart = await _cartService.GetOrCreateCart(ct);

                if (!cart.CartItems.Any())
                    throw new KeyNotFoundException("Cart is empty");

                // 2. إنشاء Order جديد
                var order = new Order
                {
                    UserId = userId,
                    PhoneNumber = phone,
                    City = city,
                    Address = address,

                    PaymentMethod = paymentMethod,
                    Status = paymentMethod == Payment.CreditCard ? OrderStatus.PendingPayment : OrderStatus.Pending,
                    DateTime = DateTime.UtcNow,
                    OrderItems = cart.CartItems.Select(ci => new OrderItem
                    {
                        ProductId = ci.ProductId,
                        ProductName = ci.Products.Name,
                        Quantity = ci.Quantity,
                        Price = ci.Products.Price,
                        TotalPrice = ci.Quantity * ci.Products.Price
                    }).ToList(),
                    TotalPrice = cart.CartItems.Sum(ci => ci.Quantity * ci.Products.Price)
                    // TotalPrice = cart.CartItems.Sum(ci => ci.Quantity * ci.Products.Price)
                };
                foreach (var item in cart.CartItems)
                {
                    var product = await _unitOfWork.ProductRepo.GetProductsByIdAsync(item.ProductId,ct);

                    if (product == null)
                        throw new KeyNotFoundException($"Product with id {item.ProductId} not found");

                    if (product.Stock < item.Quantity)
                        throw new ArgumentException($"Not enough stock for product {product.Name}");

                    product.Stock -= item.Quantity;

                    _unitOfWork.ProductRepo.UpdateProduct(product);
                }


                // 3. حفظ الطلب
                await _unitOfWork.OrderRepo.AddOrder(order,ct);
                await _unitOfWork.CompleteAsync(ct);
                await trans.CommitAsync(ct);
                 await _cartService.ClearCart(cart, ct);

                 return await _unitOfWork.OrderRepo.GetOrderById(order.Id,ct);

            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                throw ;
            }
        }



        public async Task<List<Order>> GetOrdersByUser(string userId, CancellationToken ct = default)
        {
            return await _unitOfWork.OrderRepo.GetOrdersByUserId(userId,ct);
        }

        // -----------------------------------
        // 3️⃣ جلب طلب واحد
        // -----------------------------------
        public async Task<Order> GetOrderById(int orderId, CancellationToken ct = default)
        {
            var order = await _unitOfWork.OrderRepo.GetOrderById(orderId,ct);
            if (order == null) throw new Exception("Order not found");
            return order;
        }

        // -----------------------------------
        // 4️⃣ جلب كل الطلبات (Admin)
        // -----------------------------------
        public async Task<List<Order>> GetAllOrders(CancellationToken ct = default)
        {
            return await _unitOfWork.OrderRepo.GetAllOrders(ct);
        }

        // -----------------------------------
        // 5️⃣ تغيير حالة الطلب (Admin)
        // -----------------------------------
        public async Task<Order> UpdateOrderStatus(int orderId, OrderStatus status, CancellationToken ct = default)
        {
            await _unitOfWork.OrderRepo.UpdateOrderStatus(orderId, status,ct);
            await _unitOfWork.CompleteAsync(ct);

            return await _unitOfWork.OrderRepo.GetOrderById(orderId,ct);
        }

    }
         
 }