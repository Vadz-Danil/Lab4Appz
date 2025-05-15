using System.ComponentModel.DataAnnotations;

namespace SightSeeing.Entities.DTO
{
    public class UserDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ім’я користувача обов’язкове")]
        public string Name { get; set; } = null!;

        public string Role { get; set; } = null!;

        [Required(ErrorMessage = "Пароль обов’язковий")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль має бути від 6 до 100 символів")]
        public string Password { get; set; } = null!;
    }
}