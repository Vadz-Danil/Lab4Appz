namespace SightSeeing.Entities.Entities
{
    public class Place
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Type { get; set; } = null!;

        public virtual ICollection<Review> Reviews { get; set; } = null!;
        public virtual ICollection<Question> Questions { get; set; } = null!;
        public virtual ICollection<AdditionalInfo> AdditionalInfos { get; set; } = null!;
    }
}