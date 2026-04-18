using E_Commerce_API.Models;

namespace E_Commerce_API.Reposatory.Interface
{
    public interface ICartRepo
    {
        public Task<Cart> GetCartByUserId(string userId,CancellationToken ct = default);
        public Task<List<Cart>> GetAllCarts( CancellationToken ct = default);
        public Task<Cart> AddCart(Cart cart, CancellationToken ct = default);
        public void DeleteCart(Cart cart);

         

    }
}
