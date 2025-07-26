using Microsoft.AspNetCore.Mvc;
using InvestmentSimulator.Application.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;

namespace InvestmentSimulator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IValidator<UserDto> _validator;

        public UserController(IValidator<UserDto> validator)
        {
            _validator = validator;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            var validationResult = await _validator.ValidateAsync(userDto);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            // Lógica de cadastro (ex.: salvar no banco)
            return Ok(new { Message = "User registered successfully" });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            // Lógica de autenticação
            return Ok(new { Message = "Login successful" });
        }
    }
}