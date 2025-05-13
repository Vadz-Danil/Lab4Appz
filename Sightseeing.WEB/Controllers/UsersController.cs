using Microsoft.AspNetCore.Mvc;
using SightSeeing.BLL.Interfaces;
using SightSeeing.Entities.DTO;
using SightSeeing.BLL.Exceptions;

namespace SightSeeing.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userService.AuthenticateAsync(loginDto.Username, loginDto.Password);
            if (user == null)
            {
                return BadRequest("Невірне ім’я користувача або пароль.");
            }
            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserDto userDto)
        {
            try
            {
                await _userService.AddUserAsync(userDto);
                return CreatedAtAction(nameof(GetById), new { id = userDto.Id }, userDto);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
