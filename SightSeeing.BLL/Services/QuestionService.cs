using SightSeeing.BLL.Interfaces;
using SightSeeing.Entities.DTO;
using SightSeeing.Abstraction.Interfaces;
using SightSeeing.Entities.Entities;

namespace SightSeeing.BLL.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPlaceService _placeService;
        private readonly IUserService _userService;

        public QuestionService(IUnitOfWork unitOfWork, IPlaceService placeService, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _placeService = placeService;
            _userService = userService;
        }

        public async Task AddQuestionAsync(QuestionDto questionDto)
        {
            var place = await _placeService.GetPlaceByIdAsync(questionDto.PlaceId);
            if (place == null)
            {
                throw new InvalidOperationException($"Місце з Id {questionDto.PlaceId} не існує.");
            }
            
            var user = await _userService.GetUserByIdAsync(questionDto.UserId);
            if (user == null)
            {
                throw new InvalidOperationException($"Користувач з Id {questionDto.UserId} не існує.");
            }

            var question = new Question
            {
                PlaceId = questionDto.PlaceId,
                UserId = questionDto.UserId,
                Text = questionDto.Text
            };

            await _unitOfWork.Questions.AddAsync(question);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<QuestionDto> GetQuestionByIdAsync(int id)
        {
            var question = await _unitOfWork.Questions.GetByIdAsync(id);
            if (question == null)
            {
                throw new InvalidOperationException($"Запитання з Id {id} не знайдено.");
            }

            return new QuestionDto
            {
                Id = question.Id,
                PlaceId = question.PlaceId,
                UserId = question.UserId,
                Text = question.Text
            };
        }

        public async Task<IEnumerable<QuestionDto>> GetAllQuestionsAsync()
        {
            var questions = await _unitOfWork.Questions.GetAllAsync();
            return questions.Select(q => new QuestionDto
            {
                Id = q!.Id,
                PlaceId = q.PlaceId,
                UserId = q.UserId,
                Text = q.Text
            });
        }
    }
}
