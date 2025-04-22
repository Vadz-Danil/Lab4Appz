using SightSeeing.Entities.DTO;

namespace SightSeeing.BLL.Interfaces
{
    public interface IAnswerService
    {
        Task AddAnswerAsync(AnswerDto answerDto);
        Task<IList<AnswerDto>> GetAnswersByQuestionIdAsync(int questionId);
    }
}