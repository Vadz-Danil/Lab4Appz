using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SightSeeing.BLL.Interfaces;
using SightSeeing.Entities.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SightSeeing.WEB.Pages.Reviews
{
    public class IndexModel : PageModel
    {
        private readonly IReviewService _reviewService;

        public IndexModel(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        public IEnumerable<ReviewDto> Reviews { get; set; }

        public async Task OnGetAsync()
        {
            Reviews = await _reviewService.GetAllReviewsAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _reviewService.DeleteReviewAsync(id);
            return RedirectToPage();
        }
    }
}