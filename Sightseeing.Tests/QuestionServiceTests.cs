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
    public class QuestionServiceTests : TestBase
    {
        private QuestionService _questionService;

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

            _questionService = new QuestionService(unitOfWorkMock, placeServiceMock, userServiceMock);
            
            Fixture.Customize<QuestionDto>(c => c
                .With(q => q.Text, "Valid question " + Fixture.Create<int>().ToString()));
        }

        [Test]
        public async Task AddQuestionAsync_ValidQuestion_AddsQuestion()
        {
            var questionDto = Fixture.Create<QuestionDto>();
            var place = Fixture.Create<PlaceDto>();
            var user = Fixture.Create<UserDto>();
            Kernel.Get<IPlaceService>().GetPlaceByIdAsync(questionDto.PlaceId)
                .Returns(Task.FromResult(place));
            Kernel.Get<IUserService>().GetUserByIdAsync(questionDto.UserId)
                .Returns(Task.FromResult(user));
            
            await _questionService.AddQuestionAsync(questionDto);
            
            await Kernel.Get<IUnitOfWork>().Questions.Received(1)
                .AddAsync(Arg.Is<Question>(q => q.Text == questionDto.Text));
            await Kernel.Get<IUnitOfWork>().Received(1).SaveChangesAsync();
        }

        [Test]
        public Task AddQuestionAsync_InvalidText_ThrowsValidationException()
        {
            var questionDto = Fixture.Build<QuestionDto>()
                .With(q => q.Text, "")
                .Create();
            var place = Fixture.Create<PlaceDto>();
            var user = Fixture.Create<UserDto>();
            Kernel.Get<IPlaceService>().GetPlaceByIdAsync(questionDto.PlaceId)
                .Returns(Task.FromResult(place));
            Kernel.Get<IUserService>().GetUserByIdAsync(questionDto.UserId)
                .Returns(Task.FromResult(user));
            
            var exception = Assert.ThrowsAsync<ValidationException>(
                async () => await _questionService.AddQuestionAsync(questionDto));
            Assert.That(exception.Message, Is.EqualTo("Вміст запитання не може бути порожнім."));
            return Task.CompletedTask;
        }

        [Test]
        public Task AddQuestionAsync_NonExistingPlace_ThrowsBusinessException()
        {
            var questionDto = Fixture.Create<QuestionDto>();
            Kernel.Get<IPlaceService>().GetPlaceByIdAsync(questionDto.PlaceId)
                .Returns(Task.FromResult<PlaceDto>(null!));
            
            var exception = Assert.ThrowsAsync<BusinessException>(
                async () => await _questionService.AddQuestionAsync(questionDto));
            Assert.That(exception.Message, Is.EqualTo($"Місце з Id {questionDto.PlaceId} не існує."));
            return Task.CompletedTask;
        }

        [Test]
        public Task AddQuestionAsync_NonExistingUser_ThrowsBusinessException()
        {
            var questionDto = Fixture.Create<QuestionDto>();
            var place = Fixture.Create<PlaceDto>();
            Kernel.Get<IPlaceService>().GetPlaceByIdAsync(questionDto.PlaceId)
                .Returns(Task.FromResult(place));
            Kernel.Get<IUserService>().GetUserByIdAsync(questionDto.UserId)
                .Returns(Task.FromResult<UserDto>(null!));
            
            var exception = Assert.ThrowsAsync<BusinessException>(
                async () => await _questionService.AddQuestionAsync(questionDto));
            Assert.That(exception.Message, Is.EqualTo($"Користувач з Id {questionDto.UserId} не існує."));
            return Task.CompletedTask;
        }

        [Test]
        public async Task GetQuestionByIdAsync_ExistingId_ReturnsQuestionDto()
        {
            var question = Fixture.Build<Question>().With(q => q.Id, 1).Create();
            Kernel.Get<IUnitOfWork>().Questions.GetByIdAsync(1)!
                .Returns(Task.FromResult(question));
            
            var result = await _questionService.GetQuestionByIdAsync(1);
            
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Text, Is.EqualTo(question.Text));
        }

        [Test]
        public Task GetQuestionByIdAsync_NonExistingId_ThrowsBusinessException()
        {
            Kernel.Get<IUnitOfWork>().Questions.GetByIdAsync(999)!
                .Returns(Task.FromResult<Question>(null!));
            
            var exception = Assert.ThrowsAsync<BusinessException>(
                async () => await _questionService.GetQuestionByIdAsync(999));
            Assert.That(exception.Message, Is.EqualTo("Запитання з Id 999 не знайдено."));
            return Task.CompletedTask;
        }

        [Test]
        public async Task GetAllQuestionsAsync_ReturnsAllQuestions()
        {
            var questions = Fixture.CreateMany<Question>(3).ToList();
            Kernel.Get<IUnitOfWork>().Questions.GetAllAsync()!
                .Returns(Task.FromResult(questions.AsEnumerable()));
            
            var result = await _questionService.GetAllQuestionsAsync();
            
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(3));
        }
    }
}