namespace SightSeeing.Entities.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int PlaceId { get; set; }
        public virtual Place Place { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
}