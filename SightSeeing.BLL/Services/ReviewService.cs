using AutoMapper;
using SightSeeing.Abstraction.Interfaces;
using SightSeeing.BLL.Exceptions;
using SightSeeing.BLL.Interfaces;
using SightSeeing.Entities.DTO;
using SightSeeing.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SightSeeing.BLL.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReviewDto> GetReviewByIdAsync(int id)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(id);
            if (review == null) throw new BusinessException($"Review with ID {id} not found.");
            return _mapper.Map<ReviewDto>(review);
        }

        public async Task<IEnumerable<ReviewDto>> GetAllReviewsAsync()
        {
            var reviews = await _unitOfWork.Reviews.GetAllAsync();
            return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }

        public async Task AddReviewAsync(ReviewDto reviewDto)
        {
            var review = _mapper.Map<Review>(reviewDto);
            await _unitOfWork.Reviews.AddAsync(review);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateReviewAsync(ReviewDto reviewDto)
        {
            var review = _mapper.Map<Review>(reviewDto);
            await _unitOfWork.Reviews.UpdateAsync(review);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteReviewAsync(int id)
        {
            await _unitOfWork.Reviews.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}