using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SightSeeing.BLL.Interfaces;
using System.Security.Claims;

namespace SightSeeing.WEB.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;

        public LoginModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public string Username { get; set; } = null!;

        [BindProperty]
        public string Password { get; set; } = null!;

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userService.AuthenticateAsync(Username, Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Невірне ім’я користувача або пароль.");
                return Page();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return RedirectToPage("/Index");
        }
    }
}