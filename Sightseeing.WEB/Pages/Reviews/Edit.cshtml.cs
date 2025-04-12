using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SightSeeing.BLL.Interfaces;
using SightSeeing.Entities.DTO;
using System.Threading.Tasks;

namespace SightSeeing.WEB.Pages.Reviews
{
    public class EditModel : PageModel
    {
        private readonly IReviewService _reviewService;

        public EditModel(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [BindProperty]
        public ReviewDto Review { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Review = await _reviewService.GetReviewByIdAsync(id);
            if (Review == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _reviewService.UpdateReviewAsync(Review);
            return RedirectToPage("Index");
        }
    }
}