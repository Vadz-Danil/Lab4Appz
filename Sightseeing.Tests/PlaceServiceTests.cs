using AutoFixture;
using AutoMapper;
using Ninject;
using NSubstitute;
using SightSeeing.Abstraction.Interfaces;
using SightSeeing.BLL.Exceptions;
using SightSeeing.BLL.Services;
using SightSeeing.Entities.DTO;
using SightSeeing.Entities.Entities;

namespace SightSeeing.Tests
{
    [TestFixture]
    public class PlaceServiceTests : TestBase
    {
        private PlaceService _placeService;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            var unitOfWorkMock = Substitute.For<IUnitOfWork>();
            var mapperMock = Substitute.For<IMapper>();
            Kernel.Rebind<IUnitOfWork>().ToConstant(unitOfWorkMock);
            Kernel.Rebind<IMapper>().ToConstant(mapperMock);

            _placeService = new PlaceService(unitOfWorkMock, mapperMock);

            Fixture.Customize<PlaceDto>(c => c
                .With(p => p.Name, "Valid place " + Fixture.Create<int>().ToString())
                .With(p => p.Description, "Valid description " + Fixture.Create<int>().ToString())
                .With(p => p.Type, "Monument"));
            Fixture.Customize<Place>(c => c
                .With(p => p.Name, "Valid place " + Fixture.Create<int>().ToString())
                .With(p => p.Description, "Valid description " + Fixture.Create<int>().ToString())
                .With(p => p.Type, "Monument"));
        }

        [Test]
        public async Task GetPlaceByIdAsync_ExistingId_ReturnsPlaceDto()
        {
            var place = Fixture.Create<Place>();
            var placeDto = Fixture.Create<PlaceDto>();
            Kernel.Get<IUnitOfWork>().Places.GetByIdAsync(place.Id)!.Returns(Task.FromResult(place));
            Kernel.Get<IMapper>().Map<PlaceDto>(place).Returns(placeDto);
            
            var result = await _placeService.GetPlaceByIdAsync(place.Id);
            
            Assert.That(result, Is.EqualTo(placeDto));
        }

        [Test]
        public Task GetPlaceByIdAsync_NonExistingId_ThrowsBusinessException()
        {
            Kernel.Get<IUnitOfWork>().Places.GetByIdAsync(999)!.Returns(Task.FromResult<Place>(null!));
            
            var exception = Assert.ThrowsAsync<BusinessException>(
                async () => await _placeService.GetPlaceByIdAsync(999));
            Assert.That(exception.Message, Is.EqualTo("Місце з Id 999 не знайдено."));
            return Task.CompletedTask;
        }

        [Test]
        public async Task GetAllPlacesAsync_ReturnsAllPlaces()
        {
            var places = Fixture.CreateMany<Place>(3).ToList();
            var placeDtos = Fixture.CreateMany<PlaceDto>(3).ToList();
            Kernel.Get<IUnitOfWork>().Places.GetAllAsync()!.Returns(Task.FromResult(places.AsEnumerable()));
            Kernel.Get<IMapper>().Map<IEnumerable<PlaceDto>>(places).Returns(placeDtos);
            
            var result = await _placeService.GetAllPlacesAsync();
            
            Assert.That(result, Is.EqualTo(placeDtos));
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task AddPlaceAsync_ValidPlace_AddsPlace()
        {
            var placeDto = Fixture.Create<PlaceDto>();
            var place = Fixture.Create<Place>();
            Kernel.Get<IMapper>().Map<Place>(placeDto).Returns(place);
            
            await _placeService.AddPlaceAsync(placeDto);
            
            await Kernel.Get<IUnitOfWork>().Places.Received(1).AddAsync(place);
            await Kernel.Get<IUnitOfWork>().Received(1).SaveChangesAsync();
        }

        [Test]
        public async Task UpdatePlaceAsync_ValidPlace_UpdatesPlace()
        {
            var placeDto = Fixture.Create<PlaceDto>();
            var existingPlace = Fixture.Create<Place>();
            Kernel.Get<IUnitOfWork>().Places.GetByIdAsync(placeDto.Id)!.Returns(Task.FromResult(existingPlace));
            
            await _placeService.UpdatePlaceAsync(placeDto);
            
            await Kernel.Get<IUnitOfWork>().Places.Received(1).UpdateAsync(existingPlace);
            await Kernel.Get<IUnitOfWork>().Received(1).SaveChangesAsync();
        }

        [Test]
        public Task UpdatePlaceAsync_NonExistingPlace_ThrowsBusinessException()
        {
            var placeDto = Fixture.Create<PlaceDto>();
            Kernel.Get<IUnitOfWork>().Places.GetByIdAsync(placeDto.Id)!.Returns(Task.FromResult<Place>(null!));
            
            var exception = Assert.ThrowsAsync<BusinessException>(
                async () => await _placeService.UpdatePlaceAsync(placeDto));
            Assert.That(exception.Message, Is.EqualTo($"Місце з Id {placeDto.Id} не знайдено."));
            return Task.CompletedTask;
        }

        [Test]
        public async Task DeletePlaceAsync_ValidId_DeletesPlace()
        {
            var placeId = 1;
            var place = Fixture.Create<Place>();
            Kernel.Get<IUnitOfWork>().Places.GetByIdAsync(placeId)!.Returns(Task.FromResult(place));
            
            await _placeService.DeletePlaceAsync(placeId);
            
            await Kernel.Get<IUnitOfWork>().Places.Received(1).DeleteAsync(placeId);
            await Kernel.Get<IUnitOfWork>().Received(1).SaveChangesAsync();
        }

        [Test]
        public Task DeletePlaceAsync_NonExistingId_ThrowsBusinessException()
        {
            var placeId = 999;
            Kernel.Get<IUnitOfWork>().Places.GetByIdAsync(placeId)!.Returns(Task.FromResult<Place>(null!));
            
            var exception = Assert.ThrowsAsync<BusinessException>(
                async () => await _placeService.DeletePlaceAsync(placeId));
            Assert.That(exception.Message, Is.EqualTo($"Місце з Id {placeId} не знайдено."));
            return Task.CompletedTask;
        }
    }
}