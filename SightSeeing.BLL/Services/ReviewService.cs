using SightSeeing.BLL.Interfaces;
using SightSeeing.Entities.DTO;
using SightSeeing.Abstraction.Interfaces;
using SightSeeing.Entities.Entities;
using SightSeeing.BLL.Exceptions;

namespace SightSeeing.BLL.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPlaceService _placeService;
        private readonly IUserService _userService;

        public ReviewService(IUnitOfWork unitOfWork, IPlaceService placeService, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _placeService = placeService;
            _userService = userService;
        }

        public async Task AddReviewAsync(ReviewDto reviewDto)
        {
            var place = await _placeService.GetPlaceByIdAsync(reviewDto.PlaceId);
            if (place == null)
            {
                throw new BusinessException($"Місце з Id {reviewDto.PlaceId} не існує.");
            }
            
            var user = await _userService.GetUserByIdAsync(reviewDto.UserId);
            if (user == null)
            {
                throw new BusinessException($"Користувач з Id {reviewDto.UserId} не існує.");
            }

            var review = new Review
            {
                PlaceId = reviewDto.PlaceId,
                UserId = reviewDto.UserId,
                Text = reviewDto.Text,
                Rating = reviewDto.Rating
            };

            await _unitOfWork.Reviews.AddAsync(review);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<ReviewDto>> GetAllReviewsAsync()
        {
            var reviews = await _unitOfWork.Reviews.GetAllAsync();
            return reviews.Select(r => new ReviewDto
            {
                Id = r!.Id,
                PlaceId = r.PlaceId,
                UserId = r.UserId,
                Text = r.Text,
                Rating = r.Rating
            });
        }

        public async Task<ReviewDto> GetReviewByIdAsync(int id)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(id);
            if (review == null)
            {
                throw new BusinessException($"Відгук з Id {id} не знайдено.");
            }

            return new ReviewDto
            {
                Id = review.Id,
                PlaceId = review.PlaceId,
                UserId = review.UserId,
                Text = review.Text,
                Rating = review.Rating
            };
        }

        public async Task UpdateReviewAsync(ReviewDto reviewDto)
        {
            var existingReview = await _unitOfWork.Reviews.GetByIdAsync(reviewDto.Id);
            if (existingReview == null)
            {
                throw new BusinessException($"Відгук з Id {reviewDto.Id} не знайдено.");
            }

            var place = await _placeService.GetPlaceByIdAsync(reviewDto.PlaceId);
            if (place == null)
            {
                throw new BusinessException($"Місце з Id {reviewDto.PlaceId} не існує.");
            }

            var user = await _userService.GetUserByIdAsync(reviewDto.UserId);
            if (user == null)
            {
                throw new BusinessException($"Користувач з Id {reviewDto.UserId} не існує.");
            }

            existingReview.PlaceId = reviewDto.PlaceId;
            existingReview.UserId = reviewDto.UserId;
            existingReview.Text = reviewDto.Text;
            existingReview.Rating = reviewDto.Rating;

            await _unitOfWork.Reviews.UpdateAsync(existingReview);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}