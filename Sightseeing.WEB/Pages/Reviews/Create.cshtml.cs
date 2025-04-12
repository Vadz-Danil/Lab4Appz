using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SightSeeing.BLL.Interfaces;
using SightSeeing.Entities.DTO;
using System.Threading.Tasks;

namespace SightSeeing.WEB.Pages.Reviews
{
    public class CreateModel : PageModel
    {
        private readonly IReviewService _reviewService;

        public CreateModel(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [BindProperty]
        public ReviewDto Review { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _reviewService.AddReviewAsync(Review);
            return RedirectToPage("Index");
        }
    }
}