using Microsoft.AspNetCore.Mvc;
using SightSeeing.BLL.Interfaces;
using SightSeeing.Entities.DTO;

namespace SightSeeing.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlacesController : ControllerBase
    {
        private readonly IPlaceService _placeService;

        public PlacesController(IPlaceService placeService)
        {
            _placeService = placeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var places = await _placeService.GetAllPlacesAsync();
            return Ok(places);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var place = await _placeService.GetPlaceByIdAsync(id);
            return Ok(place);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlaceDto placeDto)
        {
            await _placeService.AddPlaceAsync(placeDto);
            return CreatedAtAction(nameof(GetById), new { id = placeDto.Id }, placeDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PlaceDto placeDto)
        {
            if (id != placeDto.Id) return BadRequest("ID mismatch.");
            await _placeService.UpdatePlaceAsync(placeDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _placeService.DeletePlaceAsync(id);
            return NoContent();
        }
    }
}