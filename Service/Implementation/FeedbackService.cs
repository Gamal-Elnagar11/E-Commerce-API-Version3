using E_Commerce_API.Data;
using E_Commerce_API.Models;
using E_Commerce_API.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_API.Service.Implementation
{
    public class FeedbackService : IFeedbackService
    {

        private readonly Application _db;
        public FeedbackService(Application db)
        {
            _db = db;
        }
        public async Task AddFeedback(int Rating, string UserId, string Comment, string UserName, CancellationToken ct = default)
        {

            if (Rating < 1 || Rating > 5)
                throw new ArgumentException(" Rating Must be betwen 1 and 5");
            var feedback = new Feedback
            {
                Comment = Comment,
                UserId = UserId,
                UserName = UserName,
                Rating = Rating,
                CreatedAt = DateTime.UtcNow
            };

            await _db.Feedbacks.AddAsync(feedback,ct);
            await _db.SaveChangesAsync(ct);
        }



        public async Task<Feedback> DeleteFeedback(int id, CancellationToken ct = default)
        {
            var result = await _db.Feedbacks.FindAsync(id, ct);
            if (result == null)
                
                throw new ArgumentException($"this id {id} Not found");

              _db.Feedbacks.Remove(result);
             await  _db.SaveChangesAsync(ct);
             return result;
        }

        public async Task<List<Feedback>> GetAllFeedback(CancellationToken ct = default)
        {
            var allfeed = await _db.Feedbacks.ToListAsync(ct);
            return allfeed;
        }
    }
}
