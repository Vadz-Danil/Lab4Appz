namespace SightSeeing.Entities.Entities
{
    public class Place
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<AdditionalInfo> AdditionalInfos { get; set; }
    }
}