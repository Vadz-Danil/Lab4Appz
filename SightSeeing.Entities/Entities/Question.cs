namespace SightSeeing.Entities.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;

        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
        public int PlaceId { get; set; }
        public virtual Place Place { get; set; } = null!;
        public virtual ICollection<Answer> Answers { get; set; } = null!;
    }
}