using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SightSeeing.BLL.Interfaces;
using SightSeeing.Entities.DTO;
using System.Security.Claims;

namespace SightSeeing.WEB.Pages.Questions
{
    public class AnswerModel : PageModel
    {
        private readonly IAnswerService _answerService;
        private readonly IQuestionService _questionService;
        private readonly IPlaceService _placeService;
        private readonly IUserService _userService;

        public AnswerModel(IAnswerService answerService, IQuestionService questionService, IPlaceService placeService, IUserService userService)
        {
            _answerService = answerService;
            _questionService = questionService;
            _placeService = placeService;
            _userService = userService;
        }

        [BindProperty]
        public AnswerDto Answer { get; set; }

        public QuestionDto Question { get; set; }
        public string PlaceName { get; set; }

        public async Task<IActionResult> OnGetAsync(int questionId)
        {
            Question = await _questionService.GetQuestionByIdAsync(questionId);
            if (Question == null)
            {
                return NotFound();
            }

            var place = await _placeService.GetPlaceByIdAsync(Question.PlaceId);
            PlaceName = place != null ? place.Name : "Невідоме місце";

            Answer = new AnswerDto { QuestionId = questionId };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Question = await _questionService.GetQuestionByIdAsync(Answer.QuestionId);
                if (Question == null)
                {
                    return NotFound();
                }
                var place = await _placeService.GetPlaceByIdAsync(Question.PlaceId);
                PlaceName = place != null ? place.Name : "Невідоме місце";
                return Page();
            }
            
            Question = await _questionService.GetQuestionByIdAsync(Answer.QuestionId);
            if (Question == null)
            {
                ModelState.AddModelError("", "Запитання з таким Id не існує.");
                var place = await _placeService.GetPlaceByIdAsync(Question?.PlaceId ?? 0);
                PlaceName = place != null ? place.Name : "Невідоме місце";
                return Page();
            }
            
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                ModelState.AddModelError("", "Користувач не автентифікований.");
                var place = await _placeService.GetPlaceByIdAsync(Question.PlaceId);
                PlaceName = place != null ? place.Name : "Невідоме місце";
                return Page();
            }
            
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                ModelState.AddModelError("", "Користувач не знайдений у базі даних.");
                var place = await _placeService.GetPlaceByIdAsync(Question.PlaceId);
                PlaceName = place != null ? place.Name : "Невідоме місце";
                return Page();
            }

            Answer.UserId = userId;
            await _answerService.AddAnswerAsync(Answer);
            return RedirectToPage("/Questions/Index");
        }
    }
}
