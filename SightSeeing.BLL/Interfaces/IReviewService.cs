using SightSeeing.Entities.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SightSeeing.BLL.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewDto> GetReviewByIdAsync(int id);
        Task<IEnumerable<ReviewDto>> GetAllReviewsAsync();
        Task AddReviewAsync(ReviewDto reviewDto);
        Task UpdateReviewAsync(ReviewDto reviewDto);
        Task DeleteReviewAsync(int id);
    }
}