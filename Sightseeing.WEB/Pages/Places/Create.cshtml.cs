using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SightSeeing.BLL.Interfaces;
using SightSeeing.Entities.DTO;
using System.Threading.Tasks;

namespace SightSeeing.WEB.Pages.Places
{
    public class CreateModel : PageModel
    {
        private readonly IPlaceService _placeService;

        public CreateModel(IPlaceService placeService)
        {
            _placeService = placeService;
        }

        [BindProperty]
        public PlaceDto Place { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _placeService.AddPlaceAsync(Place);
            return RedirectToPage("Index");
        }
    }
}