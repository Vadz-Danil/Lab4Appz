using SightSeeing.Entities.DTO;

namespace SightSeeing.BLL.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> AuthenticateAsync(string username, string password);
        Task<UserDto> GetUserByIdAsync(int id);
        Task AddUserAsync(UserDto userDto);
    }
}