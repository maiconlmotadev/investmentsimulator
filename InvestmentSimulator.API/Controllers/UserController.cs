using Microsoft.AspNetCore.Mvc;
using InvestmentSimulator.Application.DTOs;
using FluentValidation;
using InvestmentSimulator.Application.Services;

namespace InvestmentSimulator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IValidator<UserDto> _validator;
        private readonly IUserService _userService;

        public UserController(IValidator<UserDto> validator, IUserService userService)
        {
            _validator = validator;
            _userService = userService;
        }

        [HttpPost("register")]
        
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            var validationResult = await _validator.ValidateAsync(userDto);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            var user = await _userService.RegisterAsync(userDto);

            return Ok(new { Message = "User registered successfully", UserId = user.Id });
        }

        [HttpPost("login")]
        
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            // Lógica de autenticação
            return Ok(new { Message = "Login successful" });
        }
    }
}