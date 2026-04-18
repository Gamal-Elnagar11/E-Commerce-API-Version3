using E_Commerce_API.Data;
using E_Commerce_API.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_API.Service.Implementation
{
    public class IdentityUsers
    {
        private readonly Application _db;

        public IdentityUsers(Application db)
        {
            _db = db;
        }


        public async Task<List<User>> GetAllUsers( CancellationToken ct = default)
        {
            var users = await _db.Users.Where(a => !a.IsDeleted).ToListAsync(ct);
            return users;
        }


        public async Task<string> DeleteUser(string email, CancellationToken ct = default)
        {
            var user = await _db.Users.FirstOrDefaultAsync(a => a.Email == email);
            if (user == null)
                return "Email Not Found";
            if (user.IsDeleted)
                return "Email Already Deleted";
            user.IsDeleted = true;
             await _db.SaveChangesAsync(ct);
            return "Deleted Successfuly";

        }


    }
}
