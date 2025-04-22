using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SightSeeing.BLL.Interfaces;
using SightSeeing.Entities.DTO;
using System.Security.Claims;

namespace SightSeeing.WEB.Pages.Reviews
{
    public class CreateModel : PageModel
    {
        private readonly IReviewService _reviewService;
        private readonly IPlaceService _placeService;
        private readonly IUserService _userService;

        public CreateModel(IReviewService reviewService, IPlaceService placeService, IUserService userService)
        {
            _reviewService = reviewService;
            _placeService = placeService;
            _userService = userService;
        }

        [BindProperty]
        public ReviewDto Review { get; set; }

        public PlaceDto Place { get; set; }

        public async Task<IActionResult> OnGetAsync(int placeId)
        {
            Place = await _placeService.GetPlaceByIdAsync(placeId);
            if (Place == null)
            {
                return NotFound();
            }

            Review = new ReviewDto { PlaceId = placeId };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Place = await _placeService.GetPlaceByIdAsync(Review.PlaceId);
                if (Place == null)
                {
                    return NotFound();
                }
                return Page();
            }
            
            Place = await _placeService.GetPlaceByIdAsync(Review.PlaceId);
            if (Place == null)
            {
                ModelState.AddModelError("", "Місце з таким Id не існує.");
                return Page();
            }
            
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                ModelState.AddModelError("", "Користувач не автентифікований.");
                Place = await _placeService.GetPlaceByIdAsync(Review.PlaceId);
                return Page();
            }
            
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                ModelState.AddModelError("", "Користувач не знайдений у базі даних.");
                Place = await _placeService.GetPlaceByIdAsync(Review.PlaceId);
                return Page();
            }

            Review.UserId = userId;
            await _reviewService.AddReviewAsync(Review);
            return RedirectToPage("/Places/Index");
        }
    }
}