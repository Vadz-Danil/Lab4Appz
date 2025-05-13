namespace SightSeeing.Entities.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual ICollection<Review> Reviews { get; set; } = null!;
        public virtual ICollection<Question> Questions { get; set; } = null!;
        public virtual ICollection<Answer> Answers { get; set; } = null!;
    }
}