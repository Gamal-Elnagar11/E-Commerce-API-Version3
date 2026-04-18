using E_Commerce_API.DTO.CartDTO;
using E_Commerce_API.Models;

namespace E_Commerce_API.Service.Interface
{
    public interface ICartService
    {
        public Task<Cart> GetCartByUserId(CancellationToken ct = default);
 
        public Task<Cart> GetOrCreateCart(CancellationToken ct = default);
         public Task<Cart> ClearCart(Cart cart, CancellationToken ct = default);


        public Task<CartItem> AddItemCart( int productid, int quantity, CancellationToken ct = default);
        public Task<CartItem> UpdateItemCarrQuantity(Cart cart,int productid, int newquantity, CancellationToken ct = default);
        public Task<CartItem> DeleteItemFromCart(Cart cart, int productid, CancellationToken ct = default);
    }
}
