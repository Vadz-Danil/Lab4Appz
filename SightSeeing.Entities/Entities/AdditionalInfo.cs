using System.ComponentModel.DataAnnotations;

namespace SightSeeing.Entities.Entities
{
    public class AdditionalInfo
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле Тип є обов'язковим")]
        public string Type { get; set; } = null!;

        [Required(ErrorMessage = "Поле Шлях до файлу є обов'язковим")]
        public string Path { get; set; } = null!;

        public int PlaceId { get; set; }
        public virtual Place? Place { get; set; }
    }
}