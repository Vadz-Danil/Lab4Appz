using AutoMapper;
using SightSeeing.Abstraction.Interfaces;
using SightSeeing.BLL.Exceptions;
using SightSeeing.BLL.Interfaces;
using SightSeeing.Entities.DTO;
using SightSeeing.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SightSeeing.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null) throw new BusinessException($"User with ID {id} not found.");
            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task AddUserAsync(UserDto userDto)
        {
            ValidationHelper.ValidateUser(userDto);
            userDto.Password = PasswordHash.HashPassword(userDto.Password);
            var user = _mapper.Map<User>(userDto);
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(UserDto userDto)
        {
            ValidationHelper.ValidateUser(userDto);
            var user = _mapper.Map<User>(userDto);
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            await _unitOfWork.Users.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}