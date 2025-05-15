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
    public class AnswerServiceTests : TestBase
    {
        private AnswerService _answerService;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            var unitOfWorkMock = Substitute.For<IUnitOfWork>();
            var questionServiceMock = Substitute.For<IQuestionService>();
            var userServiceMock = Substitute.For<IUserService>();
            Kernel.Rebind<IUnitOfWork>().ToConstant(unitOfWorkMock);
            Kernel.Rebind<IQuestionService>().ToConstant(questionServiceMock);
            Kernel.Rebind<IUserService>().ToConstant(userServiceMock);

            _answerService = new AnswerService(unitOfWorkMock, questionServiceMock, userServiceMock);
            
            Fixture.Customize<AnswerDto>(c => c
                .With(a => a.Text, "Valid answer " + Fixture.Create<int>()));
        }

        [Test]
        public async Task AddAnswerAsync_ValidAnswer_AddsAnswer()
        {
            var answerDto = Fixture.Create<AnswerDto>();
            var question = Fixture.Create<QuestionDto>();
            var user = Fixture.Create<UserDto>();
            Kernel.Get<IQuestionService>().GetQuestionByIdAsync(answerDto.QuestionId)
                .Returns(Task.FromResult(question));
            Kernel.Get<IUserService>().GetUserByIdAsync(answerDto.UserId)
                .Returns(Task.FromResult(user));
            
            await _answerService.AddAnswerAsync(answerDto);
            
            await Kernel.Get<IUnitOfWork>().Answers.Received(1)
                .AddAsync(Arg.Is<Answer>(a => a.Text == answerDto.Text));
            await Kernel.Get<IUnitOfWork>().Received(1).SaveChangesAsync();
        }

        [Test]
        public Task AddAnswerAsync_NonExistingQuestion_ThrowsBusinessException()
        {
            var answerDto = Fixture.Create<AnswerDto>();
            Kernel.Get<IQuestionService>().GetQuestionByIdAsync(answerDto.QuestionId)
                .Returns(Task.FromResult<QuestionDto>(null!));
            
            var exception = Assert.ThrowsAsync<BusinessException>(
                async () => await _answerService.AddAnswerAsync(answerDto));
            Assert.That(exception.Message, Is.EqualTo($"Запитання з Id {answerDto.QuestionId} не існує."));
            return Task.CompletedTask;
        }

        [Test]
        public Task AddAnswerAsync_NonExistingUser_ThrowsBusinessException()
        {
            var answerDto = Fixture.Create<AnswerDto>();
            var question = Fixture.Create<QuestionDto>();
            Kernel.Get<IQuestionService>().GetQuestionByIdAsync(answerDto.QuestionId)
                .Returns(Task.FromResult(question));
            Kernel.Get<IUserService>().GetUserByIdAsync(answerDto.UserId)
                .Returns(Task.FromResult<UserDto>(null!));
            
            var exception = Assert.ThrowsAsync<BusinessException>(
                async () => await _answerService.AddAnswerAsync(answerDto));
            Assert.That(exception.Message, Is.EqualTo($"Користувач з Id {answerDto.UserId} не існує."));
            return Task.CompletedTask;
        }

        [Test]
        public Task AddAnswerAsync_InvalidText_ThrowsValidationException()
        {
            var answerDto = Fixture.Build<AnswerDto>()
                .With(a => a.Text, "")
                .Create();
            var question = Fixture.Create<QuestionDto>();
            var user = Fixture.Create<UserDto>();
            Kernel.Get<IQuestionService>().GetQuestionByIdAsync(answerDto.QuestionId)
                .Returns(Task.FromResult(question));
            Kernel.Get<IUserService>().GetUserByIdAsync(answerDto.UserId)
                .Returns(Task.FromResult(user));
            
            var exception = Assert.ThrowsAsync<ValidationException>(
                async () => await _answerService.AddAnswerAsync(answerDto));
            Assert.That(exception.Message, Is.EqualTo("Вміст відповіді не може бути порожнім."));
            return Task.CompletedTask;
        }

        [Test]
        public async Task GetAnswersByQuestionIdAsync_ExistingQuestionId_ReturnsAnswers()
        {
            var questionId = 1;
            var answers = Fixture.Build<Answer>()
                .With(a => a.QuestionId, questionId)
                .CreateMany(3)
                .ToList();
            Kernel.Get<IUnitOfWork>().Answers.GetAllAsync()!
                .Returns(Task.FromResult(answers.AsEnumerable()));
            
            var result = await _answerService.GetAnswersByQuestionIdAsync(questionId);
            
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(3));
        }

        [Test]
        public async Task GetAnswersByQuestionIdAsync_NonExistingQuestionId_ReturnsEmptyList()
        {
            var questionId = 999;
            var answers = Fixture.CreateMany<Answer>(3).ToList();
            Kernel.Get<IUnitOfWork>().Answers.GetAllAsync()!
                .Returns(Task.FromResult(answers.AsEnumerable()));
            
            var result = await _answerService.GetAnswersByQuestionIdAsync(questionId);
            
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }
    }
}