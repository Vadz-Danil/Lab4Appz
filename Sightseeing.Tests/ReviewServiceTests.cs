using AutoFixture;
using Ninject;
using NSubstitute;
using SightSeeing.Abstraction.Interfaces;
using SightSeeing.BLL.Exceptions;
using SightSeeing.BLL.Interfaces;
using SightSeeing.BLL.Services;
using SightSeeing.Entities.DTO;
using SightSeeing.Entities.Entities;

namespace SightSeeing.Tests
{
    [TestFixture]
    public class ReviewServiceTests : TestBase
    {
        private ReviewService _reviewService;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            var unitOfWorkMock = Substitute.For<IUnitOfWork>();
            var placeServiceMock = Substitute.For<IPlaceService>();
            var userServiceMock = Substitute.For<IUserService>();
            Kernel.Rebind<IUnitOfWork>().ToConstant(unitOfWorkMock);
            Kernel.Rebind<IPlaceService>().ToConstant(placeServiceMock);
            Kernel.Rebind<IUserService>().ToConstant(userServiceMock);

            _reviewService = new ReviewService(unitOfWorkMock, placeServiceMock, userServiceMock);

            Fixture.Customize<ReviewDto>(c => c
                .With(r => r.Text, "Valid review " + Fixture.Create<int>())
                .With(r => r.Rating, 5));
            Fixture.Customize<Review>(c => c
                .With(r => r.Text, "Valid review " + Fixture.Create<int>())
                .With(r => r.Rating, 5));
        }

        [Test]
        public async Task AddReviewAsync_ValidReview_AddsReview()
        {
            var reviewDto = Fixture.Create<ReviewDto>();
            var place = Fixture.Create<PlaceDto>();
            var user = Fixture.Create<UserDto>();
            Kernel.Get<IPlaceService>().GetPlaceByIdAsync(reviewDto.PlaceId).Returns(Task.FromResult(place));
            Kernel.Get<IUserService>().GetUserByIdAsync(reviewDto.UserId).Returns(Task.FromResult(user));
            
            await _reviewService.AddReviewAsync(reviewDto);
            
            await Kernel.Get<IUnitOfWork>().Reviews.Received(1)
                .AddAsync(Arg.Is<Review>(r => r.Text == reviewDto.Text && r.Rating == reviewDto.Rating));
            await Kernel.Get<IUnitOfWork>().Received(1).SaveChangesAsync();
        }

        [Test]
        public Task AddReviewAsync_NonExistingPlace_ThrowsBusinessException()
        {
            var reviewDto = Fixture.Create<ReviewDto>();
            Kernel.Get<IPlaceService>().GetPlaceByIdAsync(reviewDto.PlaceId).Returns(Task.FromResult<PlaceDto>(null!));
            
            var exception = Assert.ThrowsAsync<BusinessException>(
                async () => await _reviewService.AddReviewAsync(reviewDto));
            Assert.That(exception.Message, Is.EqualTo($"Місце з Id {reviewDto.PlaceId} не існує."));
            return Task.CompletedTask;
        }

        [Test]
        public Task AddReviewAsync_NonExistingUser_ThrowsBusinessException()
        {
            var reviewDto = Fixture.Create<ReviewDto>();
            var place = Fixture.Create<PlaceDto>();
            Kernel.Get<IPlaceService>().GetPlaceByIdAsync(reviewDto.PlaceId).Returns(Task.FromResult(place));
            Kernel.Get<IUserService>().GetUserByIdAsync(reviewDto.UserId).Returns(Task.FromResult<UserDto>(null!));
            
            var exception = Assert.ThrowsAsync<BusinessException>(
                async () => await _reviewService.AddReviewAsync(reviewDto));
            Assert.That(exception.Message, Is.EqualTo($"Користувач з Id {reviewDto.UserId} не існує."));
            return Task.CompletedTask;
        }

        [Test]
        public async Task GetReviewByIdAsync_ExistingId_ReturnsReviewDto()
        {
            var review = Fixture.Create<Review>();
            Kernel.Get<IUnitOfWork>().Reviews.GetByIdAsync(review.Id)!.Returns(Task.FromResult(review));
            
            var result = await _reviewService.GetReviewByIdAsync(review.Id);
            
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(review.Id));
            Assert.That(result.Text, Is.EqualTo(review.Text));
            Assert.That(result.Rating, Is.EqualTo(review.Rating));
        }

        [Test]
        public Task GetReviewByIdAsync_NonExistingId_ThrowsBusinessException()
        {
            Kernel.Get<IUnitOfWork>().Reviews.GetByIdAsync(999)!.Returns(Task.FromResult<Review>(null!));
            
            var exception = Assert.ThrowsAsync<BusinessException>(
                async () => await _reviewService.GetReviewByIdAsync(999));
            Assert.That(exception.Message, Is.EqualTo("Відгук з Id 999 не знайдено."));
            return Task.CompletedTask;
        }

        [Test]
        public async Task GetAllReviewsAsync_ReturnsAllReviews()
        {
            var reviews = Fixture.CreateMany<Review>(3).ToList();
            Kernel.Get<IUnitOfWork>().Reviews.GetAllAsync()!.Returns(Task.FromResult(reviews.AsEnumerable()));
            
            var result = await _reviewService.GetAllReviewsAsync();
            
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task UpdateReviewAsync_ValidReview_UpdatesReview()
        {
            var reviewDto = Fixture.Create<ReviewDto>();
            var existingReview = Fixture.Create<Review>();
            var place = Fixture.Create<PlaceDto>();
            var user = Fixture.Create<UserDto>();
            Kernel.Get<IUnitOfWork>().Reviews.GetByIdAsync(reviewDto.Id)!.Returns(Task.FromResult(existingReview));
            Kernel.Get<IPlaceService>().GetPlaceByIdAsync(reviewDto.PlaceId).Returns(Task.FromResult(place));
            Kernel.Get<IUserService>().GetUserByIdAsync(reviewDto.UserId).Returns(Task.FromResult(user));
            
            await _reviewService.UpdateReviewAsync(reviewDto);
            
            await Kernel.Get<IUnitOfWork>().Reviews.Received(1)
                .UpdateAsync(Arg.Is<Review>(r => r.Text == reviewDto.Text && r.Rating == reviewDto.Rating));
            await Kernel.Get<IUnitOfWork>().Received(1).SaveChangesAsync();
        }

        [Test]
        public Task UpdateReviewAsync_NonExistingReview_ThrowsBusinessException()
        {
            var reviewDto = Fixture.Create<ReviewDto>();
            Kernel.Get<IUnitOfWork>().Reviews.GetByIdAsync(reviewDto.Id)!.Returns(Task.FromResult<Review>(null!));
            
            var exception = Assert.ThrowsAsync<BusinessException>(
                async () => await _reviewService.UpdateReviewAsync(reviewDto));
            Assert.That(exception.Message, Is.EqualTo($"Відгук з Id {reviewDto.Id} не знайдено."));
            return Task.CompletedTask;
        }

        [Test]
        public Task UpdateReviewAsync_NonExistingPlace_ThrowsBusinessException()
        {
            var reviewDto = Fixture.Create<ReviewDto>();
            var existingReview = Fixture.Create<Review>();
            Kernel.Get<IUnitOfWork>().Reviews.GetByIdAsync(reviewDto.Id)!.Returns(Task.FromResult(existingReview));
            Kernel.Get<IPlaceService>().GetPlaceByIdAsync(reviewDto.PlaceId).Returns(Task.FromResult<PlaceDto>(null!));
            
            var exception = Assert.ThrowsAsync<BusinessException>(
                async () => await _reviewService.UpdateReviewAsync(reviewDto));
            Assert.That(exception.Message, Is.EqualTo($"Місце з Id {reviewDto.PlaceId} не існує."));
            return Task.CompletedTask;
        }

        [Test]
        public Task UpdateReviewAsync_NonExistingUser_ThrowsBusinessException()
        {
            var reviewDto = Fixture.Create<ReviewDto>();
            var existingReview = Fixture.Create<Review>();
            var place = Fixture.Create<PlaceDto>();
            Kernel.Get<IUnitOfWork>().Reviews.GetByIdAsync(reviewDto.Id)!.Returns(Task.FromResult(existingReview));
            Kernel.Get<IPlaceService>().GetPlaceByIdAsync(reviewDto.PlaceId).Returns(Task.FromResult(place));
            Kernel.Get<IUserService>().GetUserByIdAsync(reviewDto.UserId).Returns(Task.FromResult<UserDto>(null!));
            
            var exception = Assert.ThrowsAsync<BusinessException>(
                async () => await _reviewService.UpdateReviewAsync(reviewDto));
            Assert.That(exception.Message, Is.EqualTo($"Користувач з Id {reviewDto.UserId} не існує."));
            return Task.CompletedTask;
        }
    }
}