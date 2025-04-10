namespace SightSeeing.Entities
{
    public class AdditionalInfo
    {
        public int Id { get; set; }
        public string Type { get; set; } 
        public string Path { get; set; }
        
        public int PlaceId { get; set; }
        public Place Place { get; set; }
    }
}