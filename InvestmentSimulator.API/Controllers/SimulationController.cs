using Microsoft.AspNetCore.Mvc;
using InvestmentSimulator.Application.Services;
using InvestmentSimulator.Domain.Models;
using FluentValidation;

namespace InvestmentSimulator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Exige autenticação
    public class SimulationController : ControllerBase
    {
        private readonly ISimulationService _simulationService;
        private readonly IValidator<Investment> _validator;

        public SimulationController(ISimulationService simulationService, IValidator<Investment> validator)
        {
            _simulationService = simulationService;
            _validator = validator;
        }

        [HttpPost("simulate")]
        public async Task<IActionResult> Simulate([FromBody] Investment investment)
        {
            var validationResult = await _validator.ValidateAsync(investment);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            var result = await _simulationService.SimulateAsync(investment);
            return Ok(result);
        }
    }
}