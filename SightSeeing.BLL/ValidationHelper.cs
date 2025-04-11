using SightSeeing.BLL.Exceptions;
using SightSeeing.Entities.DTO;
using System.Text.RegularExpressions;

namespace SightSeeing.BLL
{
    public static class ValidationHelper
    {
        public static void ValidateUser(UserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.Name))
                throw new ValidationException("User name cannot be empty.");
            if (!Regex.IsMatch(userDto.Name, @"^[a-zA-Z0-9 ]+$"))
                throw new ValidationException("User name contains invalid characters.");
            if (string.IsNullOrWhiteSpace(userDto.Password))
                throw new ValidationException("Password cannot be empty.");
        }
    }
}