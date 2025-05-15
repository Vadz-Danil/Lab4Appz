using SightSeeing.BLL.Interfaces;
using SightSeeing.Entities.DTO;
using SightSeeing.Abstraction.Interfaces;
using SightSeeing.Entities.Entities;
using SightSeeing.BLL.Exceptions;

namespace SightSeeing.BLL.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQuestionService _questionService;
        private readonly IUserService _userService;

        public AnswerService(IUnitOfWork unitOfWork, IQuestionService questionService, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _questionService = questionService;
            _userService = userService;
        }

        public async Task AddAnswerAsync(AnswerDto answerDto)
        {
            if (string.IsNullOrEmpty(answerDto.Text))
                throw new ValidationException("Вміст відповіді не може бути порожнім.");

            var question = await _questionService.GetQuestionByIdAsync(answerDto.QuestionId);
            if (question == null)
                throw new BusinessException($"Запитання з Id {answerDto.QuestionId} не існує.");

            var user = await _userService.GetUserByIdAsync(answerDto.UserId);
            if (user == null)
                throw new BusinessException($"Користувач з Id {answerDto.UserId} не існує.");

            var answer = new Answer
            {
                QuestionId = answerDto.QuestionId,
                UserId = answerDto.UserId,
                Text = answerDto.Text
            };

            await _unitOfWork.Answers.AddAsync(answer);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IList<AnswerDto>> GetAnswersByQuestionIdAsync(int questionId)
        {
            var answers = await _unitOfWork.Answers.GetAllAsync();
            return answers
                .Where(a => a!.QuestionId == questionId)
                .Select(a => new AnswerDto
                {
                    Id = a!.Id,
                    QuestionId = a.QuestionId,
                    UserId = a.UserId,
                    Text = a.Text
                })
                .ToList();
        }
    }
}