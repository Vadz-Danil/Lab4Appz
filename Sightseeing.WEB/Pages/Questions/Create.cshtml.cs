using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SightSeeing.BLL.Interfaces;
using SightSeeing.Entities.DTO;
using System.Security.Claims;

namespace SightSeeing.WEB.Pages.Questions
{
    public class CreateModel : PageModel
    {
        private readonly IQuestionService _questionService;
        private readonly IPlaceService _placeService;
        private readonly IUserService _userService;

        public CreateModel(IQuestionService questionService, IPlaceService placeService, IUserService userService)
        {
            _questionService = questionService;
            _placeService = placeService;
            _userService = userService;
        }

        [BindProperty]
        public QuestionDto Question { get; set; } = null!;

        public PlaceDto Place { get; set; } = null!;

        public string ErrorMessage { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int placeId)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                ErrorMessage = "Ви не автентифіковані. Увійдіть, щоб додавати запитання.";
                return Page();
            }
            
            if (!User.IsInRole("Manager"))
            {
                ErrorMessage = "Немає доступу. Тільки менеджери можуть додавати запитання.";
                return Page();
            }
            
            Place = await _placeService.GetPlaceByIdAsync(placeId);
            if (Place == null)
            {
                return NotFound();
            }

            Question = new QuestionDto { PlaceId = placeId };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                ErrorMessage = "Ви не автентифіковані. Увійдіть, щоб додавати запитання.";
                return Page();
            }
            
            if (!User.IsInRole("Manager"))
            {
                ErrorMessage = "Немає доступу. Тільки менеджери можуть додавати запитання.";
                return Page();
            }

            if (!ModelState.IsValid)
            {
                Place = await _placeService.GetPlaceByIdAsync(Question.PlaceId);
                if (Place == null)
                {
                    return NotFound();
                }
                return Page();
            }
            
            Place = await _placeService.GetPlaceByIdAsync(Question.PlaceId);
            if (Place == null)
            {
                ModelState.AddModelError("", "Місце з таким Id не існує.");
                return Page();
            }
            
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                ModelState.AddModelError("", "Користувач не автентифікований.");
                Place = await _placeService.GetPlaceByIdAsync(Question.PlaceId);
                return Page();
            }
            
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                ModelState.AddModelError("", "Користувач не знайдений у базі даних.");
                Place = await _placeService.GetPlaceByIdAsync(Question.PlaceId);
                return Page();
            }

            Question.UserId = userId;
            await _questionService.AddQuestionAsync(Question);
            return RedirectToPage("/Places/Index");
        }
    }
}
