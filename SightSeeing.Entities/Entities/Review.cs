namespace SightSeeing.Entities.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public int Rating { get; set; }
        
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public int PlaceId { get; set; }
        public virtual Place Place { get; set; } = null!;
    }
}