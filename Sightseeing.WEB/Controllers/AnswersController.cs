using Microsoft.AspNetCore.Mvc;
using SightSeeing.BLL.Exceptions;
using SightSeeing.BLL.Interfaces;
using SightSeeing.Entities.DTO;

namespace SightSeeing.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private readonly IAnswerService _answerService;

        public AnswersController(IAnswerService answerService)
        {
            _answerService = answerService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnswerDto>>> GetAnswersByQuestionId(int questionId)
        {
            try
            {
                var answers = await _answerService.GetAnswersByQuestionIdAsync(questionId);
                return Ok(answers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
            }
        }
        
        [HttpPost("{questionId}/answers")]
        public async Task<ActionResult<AnswerDto>> CreateAnswer(int questionId, [FromBody] AnswerDto answerDto)
        {
            if (answerDto == null || answerDto.QuestionId != questionId)
            {
                return BadRequest("Дані відповіді не можуть бути порожніми або Id запитання не співпадає.");
            }

            try
            {
                await _answerService.AddAnswerAsync(answerDto);
                return CreatedAtAction(nameof(GetAnswersByQuestionId), new { questionId = answerDto.QuestionId }, answerDto);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
            }
        }
    }
}