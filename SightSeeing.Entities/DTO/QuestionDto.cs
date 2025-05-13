namespace SightSeeing.Entities.DTO;

public class QuestionDto
{
    public int Id { get; set; }
    public string Text { get; set; } = null!;
    public int UserId { get; set; }
    public int PlaceId { get; set; }
}