using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SightSeeing.BLL.Interfaces;
using SightSeeing.Entities.DTO;
using SightSeeing.Abstraction.Interfaces;
using SightSeeing.Entities.Entities;

namespace SightSeeing.WEB.Pages.Places
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IPlaceService _placeService;
        private readonly IReviewService _reviewService;
        private readonly IUnitOfWork _unitOfWork;

        public DetailsModel(IPlaceService placeService, IReviewService reviewService, IUnitOfWork unitOfWork)
        {
            _placeService = placeService;
            _reviewService = reviewService;
            _unitOfWork = unitOfWork;
        }

        public PlaceDto Place { get; set; } = null!;
        public IEnumerable<ReviewDto> Reviews { get; set; } = null!;
        public IEnumerable<Question?> Questions { get; set; } = null!;
        public IEnumerable<AdditionalInfo?> AdditionalInfos { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Place = await _placeService.GetPlaceByIdAsync(id);
            if (Place == null)
            {
                return NotFound();
            }
            Reviews = await _reviewService.GetAllReviewsAsync();
            Reviews = Reviews.Where(r => r.PlaceId == id);
            Questions = await _unitOfWork.Questions.GetAllAsync();
            Questions = Questions.Where(q => q!.PlaceId == id);
            AdditionalInfos = await _unitOfWork.AdditionalInfos.GetAllAsync();
            AdditionalInfos = AdditionalInfos.Where(ai => ai!.PlaceId == id);
            return Page();
        }
    }
}