using E_Commerce_API.Models;
using E_Commerce_API.UnitOfWork;

namespace E_Commerce_API.Service.Interface
{
    public interface ICategoryService
    {
        public Task<List<Category>> GetAllCateory(CancellationToken ct = default);
        public Task <List<Category>> GetAllCateoryWithProducts(CancellationToken ct = default);
        public Task<Category> GetCategoryByIdAsync(int id, CancellationToken ct = default);
        public Task<Category> AddCategoryAsync(Category category, CancellationToken ct = default);
        public Task<Category> UpdateCategoryAsync(int id ,Category category , CancellationToken ct = default);
        public Task<Category> DeleteCategoryAsync(int id, CancellationToken ct = default);
        public Task<List<Category>>  Search (string name, CancellationToken ct = default);
        public Task<List<Category>> SearchWithProducts(string name, CancellationToken ct = default);  
    }
}
