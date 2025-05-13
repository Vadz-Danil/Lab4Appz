namespace SightSeeing.Entities.DTO;

public class AnswerDto
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public int UserId { get; set; }
    public string Text { get; set; } = null!;
}