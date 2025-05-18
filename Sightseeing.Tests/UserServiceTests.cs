using AutoFixture;
using AutoMapper;
using Ninject;
using NSubstitute;
using SightSeeing.Abstraction.Interfaces;
using SightSeeing.BLL;
using SightSeeing.BLL.Exceptions;
using SightSeeing.BLL.Services;
using SightSeeing.Entities.DTO;
using SightSeeing.Entities.Entities;

namespace SightSeeing.Tests
{
    [TestFixture]
    public class UserServiceTests : TestBase
    {
        private UserService _userService;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            var unitOfWorkMock = Substitute.For<IUnitOfWork>();
            var mapperMock = Substitute.For<IMapper>();
            Kernel.Rebind<IUnitOfWork>().ToConstant(unitOfWorkMock);
            Kernel.Rebind<IMapper>().ToConstant(mapperMock);

            _userService = new UserService(unitOfWorkMock, mapperMock);
            
            Fixture.Customize<UserDto>(c => c
                .With(u => u.Name, "ValidUser" + Fixture.Create<uint>())
                .With(u => u.Password, "ValidPass123")
                .With(u => u.Role, "User"));
            Fixture.Customize<User>(c => c
                .With(u => u.Name, "ValidUser" + Fixture.Create<uint>())
                .With(u => u.Password, PasswordHash.HashPassword("ValidPass123"))
                .With(u => u.Role, "User"));
        }

        [Test]
        public async Task AuthenticateAsync_ValidCredentials_ReturnsUserDto()
        {
            var user = Fixture.Create<User>();
            var userDto = Fixture.Create<UserDto>();
            var username = user.Name;
            var password = "ValidPass123";
            Kernel.Get<IUnitOfWork>().Users.GetAllAsync()!
                .Returns(Task.FromResult(new[] { user }.AsEnumerable()));
            Kernel.Get<IMapper>().Map<UserDto>(user).Returns(userDto);
            
            var result = await _userService.AuthenticateAsync(username, password);
            
            Assert.That(result, Is.EqualTo(userDto));
        }

        [Test]
        public async Task AuthenticateAsync_InvalidCredentials_ReturnsNull()
        {
            var user = Fixture.Create<User>();
            var username = user.Name;
            var password = "WrongPass";
            Kernel.Get<IUnitOfWork>().Users.GetAllAsync()!
                .Returns(Task.FromResult(new[] { user }.AsEnumerable()));
            
            var result = await _userService.AuthenticateAsync(username, password);
            
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task AuthenticateAsync_NonExistingUser_ReturnsNull()
        {
            var username = "NonExisting";
            var password = "ValidPass123";
            Kernel.Get<IUnitOfWork>().Users.GetAllAsync()!
                .Returns(Task.FromResult(Enumerable.Empty<User>()));
            
            var result = await _userService.AuthenticateAsync(username, password);
            
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetUserByIdAsync_ExistingId_ReturnsUserDto()
        {
            var user = Fixture.Create<User>();
            var userDto = Fixture.Create<UserDto>();
            Kernel.Get<IUnitOfWork>().Users.GetByIdAsync(user.Id)!.Returns(Task.FromResult(user));
            Kernel.Get<IMapper>().Map<UserDto>(user).Returns(userDto);
            
            var result = await _userService.GetUserByIdAsync(user.Id);
            
            Assert.That(result, Is.EqualTo(userDto));
        }

        [Test]
        public Task GetUserByIdAsync_NonExistingId_ThrowsBusinessException()
        {
            Kernel.Get<IUnitOfWork>().Users.GetByIdAsync(999)!.Returns(Task.FromResult<User>(null!));
            
            var exception = Assert.ThrowsAsync<BusinessException>(
                async () => await _userService.GetUserByIdAsync(999));
            Assert.That(exception.Message, Is.EqualTo("User with ID 999 not found."));
            return Task.CompletedTask;
        }

        [Test]
        public async Task AddUserAsync_ValidUser_AddsUser()
        {
            var userDto = Fixture.Create<UserDto>();
            var user = Fixture.Create<User>();
            user.Password = PasswordHash.HashPassword(userDto.Password);
            Kernel.Get<IMapper>().Map<User>(userDto).Returns(user);
            
            await _userService.AddUserAsync(userDto);
            
            await Kernel.Get<IUnitOfWork>().Users.Received(1).AddAsync(Arg.Is<User>(u => u.Password == user.Password));
            await Kernel.Get<IUnitOfWork>().Received(1).SaveChangesAsync();
        }

        [Test]
        public Task AddUserAsync_EmptyName_ThrowsValidationException()
        {
            var userDto = Fixture.Build<UserDto>()
                .With(u => u.Name, "")
                .Create();
            
            var exception = Assert.ThrowsAsync<ValidationException>(
                async () => await _userService.AddUserAsync(userDto));
            Assert.That(exception.Message, Is.EqualTo("User name cannot be empty."));
            return Task.CompletedTask;
        }
        
        [Test]
        public Task AddUserAsync_EmptyPassword_ThrowsValidationException()
        {
            var userDto = Fixture.Build<UserDto>()
                .With(u => u.Name, "ValidUser123")
                .With(u => u.Password, "")
                .Create();
            
            var exception = Assert.ThrowsAsync<ValidationException>(
                async () => await _userService.AddUserAsync(userDto));
            Assert.That(exception.Message, Is.EqualTo("Password cannot be empty."));
            return Task.CompletedTask;
        }

        [Test]
        public Task AddUserAsync_InvalidNameCharacters_ThrowsValidationException()
        {
            var userDto = Fixture.Build<UserDto>()
                .With(u => u.Name, "Invalid@Name")
                .Create();
            
            var exception = Assert.ThrowsAsync<ValidationException>(
                async () => await _userService.AddUserAsync(userDto));
            Assert.That(exception.Message, Is.EqualTo("User name contains invalid characters."));
            return Task.CompletedTask;
        }
    }
}