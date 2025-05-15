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
            var place = await _unitOfWork.Places.GetByIdAsync(id);
            if (place == null)
                throw new BusinessException($"Місце з Id {id} не знайдено.");
            return _mapper.Map<PlaceDto>(place);
        }

        public async Task<IEnumerable<PlaceDto>> GetAllPlacesAsync()
        {
            var places = await _unitOfWork.Places.GetAllAsync();
            return _mapper.Map<IEnumerable<PlaceDto>>(places);
        }

        public async Task AddPlaceAsync(PlaceDto placeDto)
        {
            if (string.IsNullOrEmpty(placeDto.Name))
                throw new ValidationException("Назва місця не може бути порожньою.");

            var place = _mapper.Map<Place>(placeDto);
            await _unitOfWork.Places.AddAsync(place);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdatePlaceAsync(PlaceDto placeDto)
        {
            var existingPlace = await _unitOfWork.Places.GetByIdAsync(placeDto.Id);
            if (existingPlace == null)
            {
                throw new BusinessException($"Місце з Id {placeDto.Id} не знайдено.");
            }

            existingPlace.Name = placeDto.Name;
            existingPlace.Description = placeDto.Description;
            existingPlace.Type = placeDto.Type;

            await _unitOfWork.Places.UpdateAsync(existingPlace);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeletePlaceAsync(int id)
        {
            var place = await _unitOfWork.Places.GetByIdAsync(id);
            if (place == null)
                throw new BusinessException($"Місце з Id {id} не знайдено.");

            await _unitOfWork.Places.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
