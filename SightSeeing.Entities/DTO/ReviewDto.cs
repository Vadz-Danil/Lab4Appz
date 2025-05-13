namespace SightSeeing.Entities.DTO;

public class ReviewDto
{
    public int Id { get; set; }
    public string Text { get; set; } = null!;
    public int Rating { get; set; }
    public int UserId { get; set; }
    public int PlaceId { get; set; }
}