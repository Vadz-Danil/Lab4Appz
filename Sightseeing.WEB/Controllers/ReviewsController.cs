using Microsoft.AspNetCore.Mvc;
using SightSeeing.BLL.Interfaces;
using SightSeeing.Entities.DTO;

namespace SightSeeing.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            return Ok(review);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReviewDto reviewDto)
        {
            await _reviewService.AddReviewAsync(reviewDto);
            return CreatedAtAction(nameof(GetById), new { id = reviewDto.Id }, reviewDto);
        }
    }
}