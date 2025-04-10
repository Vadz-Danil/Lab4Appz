namespace SightSeeing.Entities
{
    public class Place
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Type { get; set; }
        
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Question> Questions { get; set; }
        public ICollection<AdditionalInfo> AdditionalInfos { get; set; }
    }
}