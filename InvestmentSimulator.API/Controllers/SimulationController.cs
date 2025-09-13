using Microsoft.AspNetCore.Mvc;
using InvestmentSimulator.Application.Services.Simulation.Deposit;
using InvestmentSimulator.Domain.Models.Simulation.Deposit;
using FluentValidation;
using System.Threading.Tasks;

namespace InvestmentSimulator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SimulationController : ControllerBase
    {
        private readonly IDepositSimulatorService _depositSimulatorService;
        private readonly IValidator<DepositInput> _validator;

        public SimulationController(
            IDepositSimulatorService depositSimulatorService,
            IValidator<DepositInput> validator)
        {
            _depositSimulatorService = depositSimulatorService;
            _validator = validator;
        }

        [HttpPost("simulate")]
        public async Task<IActionResult> SimulateAsync([FromBody] DepositInput investment)
        {
            var validationResult = await _validator.ValidateAsync(investment);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            var result = await _depositSimulatorService.SimulateAsync(investment);
            return Ok(result);
        }
    }
}
