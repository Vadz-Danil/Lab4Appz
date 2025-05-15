using Microsoft.AspNetCore.Mvc.RazorPages;
using SightSeeing.BLL.Interfaces;
using SightSeeing.Entities.DTO;

namespace SightSeeing.WEB.Pages.Questions
{
    public class IndexModel : PageModel
    {
        private readonly IQuestionService _questionService;
        private readonly IAnswerService _answerService;
        private readonly IPlaceService _placeService;

        public IndexModel(IQuestionService questionService, IAnswerService answerService, IPlaceService placeService)
        {
            _questionService = questionService;
            _answerService = answerService;
            _placeService = placeService;
        }

        public IList<QuestionDto> Questions { get; set; } = null!;
        public IDictionary<int, IList<AnswerDto>> AnswersByQuestionId { get; set; } = null!;
        public IDictionary<int, string> PlaceNames { get; set; } = null!;

        public async Task OnGetAsync()
        {
            Questions = new List<QuestionDto>(await _questionService.GetAllQuestionsAsync());
            AnswersByQuestionId = new Dictionary<int, IList<AnswerDto>>();
            PlaceNames = new Dictionary<int, string>();

            foreach (var question in Questions)
            {
                var answers = await _answerService.GetAnswersByQuestionIdAsync(question.Id);
                AnswersByQuestionId[question.Id] = answers;
                
                var place = await _placeService.GetPlaceByIdAsync(question.PlaceId);
                PlaceNames[question.Id] = place != null ? place.Name : "Невідоме місце";
            }
        }
    }
}