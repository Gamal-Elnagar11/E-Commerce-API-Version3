using E_Commerce_API.Models;

namespace E_Commerce_API.Service.Interface
{
    public interface IProductService
    {
       public IQueryable<Product> GetAllProducts();
       public Task<Product> GetProductByIdAsync(int id, CancellationToken ct = default);
       public Task<Product> UpdateProductAsync(int id , Product product, CancellationToken ct = default);
        public Task<Product> UpdateStock(int id ,int stock, CancellationToken ct = default);
       public Task<Product> AddProductAsync(Product product, CancellationToken ct = default);
       public Task<Product> DeleteProductAsync(int id, CancellationToken ct = default);
        public Task<List<Product>> Search(string name, CancellationToken ct = default);

        public Task<bool> CategoryExistsAsync(int categoryId, CancellationToken ct = default);

       


    }
}
