using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SightSeeing.BLL.Interfaces;
using SightSeeing.Entities.DTO;

namespace SightSeeing.WEB.Pages.Places
{
    public class EditModel : PageModel
    {
        private readonly IPlaceService _placeService;
        public EditModel(IPlaceService placeService)
        {
            _placeService = placeService;
        }
        [BindProperty]
        public PlaceDto Place { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Place = await _placeService.GetPlaceByIdAsync(id);
            if (Place == null)
            {
                Console.WriteLine("Місце не знайдено");
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Помилка: {error.ErrorMessage}");
                }
                return Page();
            }
            try
            {
                await _placeService.UpdatePlaceAsync(Place);
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Виникла помилка при оновленні місця. Спробуйте ще раз.");
                return Page();
            }
        }
    }
}