namespace SightSeeing.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedDate { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; }
        public int PlaceId { get; set; }
        public Place Place { get; set; }
    }
}