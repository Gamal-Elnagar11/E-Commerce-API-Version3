using E_Commerce_API.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_API.GenaricRepo
{
    public class GenaricRepo<T> : IGenaricRepo<T> where T : class
    {

        private readonly Application _db;
        private readonly DbSet<T> _Set;

        public GenaricRepo(Application db)
        {
            _db = db; 
            _Set = _db.Set<T>();
        }


        public IQueryable<T> GetAll()
        {
             
            return _Set;
        }



        public async Task<T> GetByIdAsync(int id, CancellationToken ct = default)
        {
             return await _Set.FindAsync(id, ct);
        }




        public async Task<T> AddAsync(T value, CancellationToken ct = default)
        {
            await _Set.AddAsync(value, ct);
            return value;
        }





        public void Update(T value)
        {
            _Set.Update(value);
        }


        public void Delete(T value)
        {
            _Set.Remove(value);
        }

    }
}

