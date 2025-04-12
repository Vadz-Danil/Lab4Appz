using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SightSeeing.BLL.Interfaces;
using SightSeeing.Entities.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SightSeeing.WEB.Pages.Places
{
    public class IndexModel : PageModel
    {
        private readonly IPlaceService _placeService;

        public IndexModel(IPlaceService placeService)
        {
            _placeService = placeService;
        }

        public IEnumerable<PlaceDto> Places { get; set; }

        public async Task OnGetAsync()
        {
            Places = await _placeService.GetAllPlacesAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _placeService.DeletePlaceAsync(id);
            return RedirectToPage();
        }
    }
}