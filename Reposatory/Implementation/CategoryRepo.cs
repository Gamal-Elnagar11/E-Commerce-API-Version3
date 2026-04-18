using E_Commerce_API.Data;
using E_Commerce_API.Models;
using E_Commerce_API.Reposatory.Implementation;
using E_Commerce_API.Reposatory.Interface;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_API.Reposatory.Implementation
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly Application _db;

        public CategoryRepo(Application db)
        {
            _db = db;
        }

        public async Task<Category> AddAsync(Category category, CancellationToken ct = default)
        {
              await _db.Categories.AddAsync(category,ct);
            return category;
        }

        public void DeleteCategory(Category product)
        {
            _db.Categories.Remove(product);
        }

        public async Task<Category> GeCategoriesByIdAsync(int id, CancellationToken ct = default)
        {
            return await _db.Categories.Include(a => a.Products).FirstOrDefaultAsync( a=> a.Id == id,ct);
        }

        public async Task<List<Category>> GetAllCategoriesAsync(CancellationToken ct = default)
        {
            return await _db.Categories.AsNoTracking().ToListAsync(ct);
        }

        public async Task<List<Category>> GetAllCategoriesWithProducts(CancellationToken ct = default)
        {
            return await _db.Categories.Include(c => c.Products).ToListAsync(ct);
        }

        public async Task<List<Category>> Search(string name, CancellationToken ct = default)
        {
             return await _db.Categories.Where(a => a.Name.Contains(name)).ToListAsync(ct);
        }

        public async Task<List<Category>> SearchwithProducts(string name, CancellationToken ct = default)
        {
             return await _db.Categories.Include(a => a.Products).Where(a => a.Name.Contains(name)).ToListAsync(ct);
        }

        public void UpdateCategory(Category product)
        {
               
            _db.Update(product);
        }
    }
} 