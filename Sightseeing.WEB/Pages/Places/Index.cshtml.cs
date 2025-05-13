using Microsoft.AspNetCore.Mvc.RazorPages;
using SightSeeing.BLL.Interfaces;
using SightSeeing.Entities.DTO;

namespace SightSeeing.WEB.Pages.Places
{
    public class IndexModel : PageModel
    {
        private readonly IPlaceService _placeService;
        public IndexModel(IPlaceService placeService)
        {
            _placeService = placeService;
        }
        public IEnumerable<PlaceDto> Places { get; set; } = null!;

        public async Task OnGetAsync()
        {
            Places = await _placeService.GetAllPlacesAsync();
        }
    }
}