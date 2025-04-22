using Microsoft.AspNetCore.Mvc.RazorPages;
using SightSeeing.BLL.Interfaces;
using SightSeeing.Entities.DTO;

namespace SightSeeing.WEB.Pages.Reviews
{
    public class IndexModel : PageModel
    {
        private readonly IReviewService _reviewService;
        private readonly IPlaceService _placeService;

        public IndexModel(IReviewService reviewService, IPlaceService placeService)
        {
            _reviewService = reviewService;
            _placeService = placeService;
        }

        public IList<ReviewDto> Reviews { get; set; }
        public Dictionary<int, string> PlaceNames { get; set; }

        public async Task OnGetAsync()
        {
            Reviews = (await _reviewService.GetAllReviewsAsync()).ToList();
            PlaceNames = new Dictionary<int, string>();
            
            var placeIds = Reviews.Select(r => r.PlaceId).Distinct();
            foreach (var placeId in placeIds)
            {
                var place = await _placeService.GetPlaceByIdAsync(placeId);
                if (place != null)
                {
                    PlaceNames[placeId] = place.Name;
                }
                else
                {
                    PlaceNames[placeId] = "Невідоме місце";
                }
            }
        }
    }
}