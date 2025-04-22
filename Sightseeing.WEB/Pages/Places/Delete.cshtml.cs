using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SightSeeing.BLL.Interfaces;
using SightSeeing.Entities.DTO;

namespace SightSeeing.WEB.Pages.Places
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly IPlaceService _placeService;

        public DeleteModel(IPlaceService placeService)
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
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _placeService.DeletePlaceAsync(Place.Id);
            return RedirectToPage("Index");
        }
    }
}