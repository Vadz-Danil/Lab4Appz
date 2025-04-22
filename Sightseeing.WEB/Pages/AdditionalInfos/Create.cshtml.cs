using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SightSeeing.Abstraction.Interfaces;
using SightSeeing.Entities.Entities;

namespace SightSeeing.WEB.Pages.AdditionalInfos
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public AdditionalInfo AdditionalInfo { get; set; }

        public IActionResult OnGet(int placeId)
        {
            AdditionalInfo = new AdditionalInfo { PlaceId = placeId };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _unitOfWork.AdditionalInfos.AddAsync(AdditionalInfo);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToPage("/Places/Index");
        }
    }
}