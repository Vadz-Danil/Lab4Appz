using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SightSeeing.BLL.Interfaces;
using SightSeeing.Entities.DTO;
using System.Threading.Tasks;

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
            if (Place == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _placeService.UpdatePlaceAsync(Place);
            return RedirectToPage("Index");
        }
    }
}