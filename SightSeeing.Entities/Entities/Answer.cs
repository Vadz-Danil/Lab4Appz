namespace SightSeeing.Entities.Entities
{
    public class Answer
    {
        public int Id { get; set; }
        public string Text { get; set; }
        
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }
}