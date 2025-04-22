using AutoMapper;
using SightSeeing.Abstraction.Interfaces;
using SightSeeing.BLL.Exceptions;
using SightSeeing.BLL.Interfaces;
using SightSeeing.Entities.DTO;
using SightSeeing.Entities.Entities;

namespace SightSeeing.BLL.Services
{
    public class PlaceService : IPlaceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlaceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PlaceDto> GetPlaceByIdAsync(int id)
        {
            Console.WriteLine($"GetPlaceByIdAsync викликано з id: {id}");
            var place = await _unitOfWork.Places.GetByIdAsync(id);
            if (place == null)
            {
                Console.WriteLine($"Місце з Id {id} не знайдено в базі даних.");
                throw new BusinessException($"Place with ID {id} not found.");
            }
            var placeDto = _mapper.Map<PlaceDto>(place);
            Console.WriteLine($"Місце знайдено: Id={placeDto.Id}, Name={placeDto.Name}");
            return placeDto;
        }

        public async Task<IEnumerable<PlaceDto>> GetAllPlacesAsync()
        {
            var places = await _unitOfWork.Places.GetAllAsync();
            return _mapper.Map<IEnumerable<PlaceDto>>(places);
        }

        public async Task AddPlaceAsync(PlaceDto placeDto)
        {
            var place = _mapper.Map<Place>(placeDto);
            await _unitOfWork.Places.AddAsync(place);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdatePlaceAsync(PlaceDto placeDto)
        {
            var place = _mapper.Map<Place>(placeDto);
            await _unitOfWork.Places.UpdateAsync(place);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeletePlaceAsync(int id)
        {
            await _unitOfWork.Places.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
