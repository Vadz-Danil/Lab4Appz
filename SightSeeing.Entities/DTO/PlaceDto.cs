using System.ComponentModel.DataAnnotations;

namespace SightSeeing.Entities.DTO;

public class PlaceDto
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Назва є обов'язковою")]
    public string Name { get; set; } = null!;
    [Required(ErrorMessage = "Опис є обов'язковим")]
    public string Description { get; set; } = null!;
    [Required(ErrorMessage = "Тип є обов'язковим")]
    public string Type { get; set; } = null!;
}