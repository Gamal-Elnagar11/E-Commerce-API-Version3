using E_Commerce_API.Models;

namespace E_Commerce_API.Service.Interface
{
    public interface IFeedbackService
    {
        public Task AddFeedback(int Rating, string UserId, string Comment, string UserName, CancellationToken ct = default);
        public Task<List<Feedback>> GetAllFeedback(CancellationToken ct = default);
        public Task<Feedback> DeleteFeedback(int id, CancellationToken ct = default);
    }
}
