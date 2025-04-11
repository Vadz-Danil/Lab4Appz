using SightSeeing.Entities.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SightSeeing.BLL.Interfaces
{
    public interface IPlaceService
    {
        Task<PlaceDto> GetPlaceByIdAsync(int id);
        Task<IEnumerable<PlaceDto>> GetAllPlacesAsync();
        Task AddPlaceAsync(PlaceDto placeDto);
        Task UpdatePlaceAsync(PlaceDto placeDto);
        Task DeletePlaceAsync(int id);
    }
}