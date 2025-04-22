using SightSeeing.Entities.DTO;

namespace SightSeeing.BLL.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> AuthenticateAsync(string username, string password);
        Task<UserDto> GetUserByIdAsync(int id);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task AddUserAsync(UserDto userDto);
        Task UpdateUserAsync(UserDto userDto);
        Task DeleteUserAsync(int id);
    }
}