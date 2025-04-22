using SightSeeing.Entities.DTO;

namespace SightSeeing.BLL.Interfaces
{
    public interface IQuestionService
    {
        Task<QuestionDto> GetQuestionByIdAsync(int id);
        Task<IEnumerable<QuestionDto>> GetAllQuestionsAsync();
        Task AddQuestionAsync(QuestionDto questionDto);
    }
}