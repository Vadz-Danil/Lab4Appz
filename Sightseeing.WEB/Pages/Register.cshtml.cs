using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SightSeeing.BLL.Exceptions;
using SightSeeing.BLL.Interfaces;
using SightSeeing.Entities.DTO;

namespace SightSeeing.WEB.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IUserService _userService;

        public RegisterModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public UserDto User { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            if (User.Role != "User" && User.Role != "Manager")
            {
                ModelState.AddModelError("User.Role", "Недійсна роль. Оберіть 'Користувач' або 'Менеджер'.");
                return Page();
            }

            try
            {
                User.Id = 0;
                await _userService.AddUserAsync(User);
                return RedirectToPage("/Login");
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Виникла помилка під час реєстрації: {ex.Message}");
                return Page();
            }
        }
    }
}