
using System.Runtime;
using E_Commerce_API.Models;

namespace E_Commerce_API.GenaricRepo
{
    public interface IGenaricRepo<T> where T : class
    {
        public IQueryable<T> GetAll();
        public Task<T> GetByIdAsync(int id, CancellationToken ct = default);
        public Task<T> AddAsync(T value, CancellationToken ct = default);
        public void Update (T value);
        public void Delete (T value);
    }
}
