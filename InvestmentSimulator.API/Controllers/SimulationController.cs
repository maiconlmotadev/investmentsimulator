using Microsoft.AspNetCore.Mvc;
using InvestmentSimulator.Application.Services;
using InvestmentSimulator.Domain.Models;
using FluentValidation;
using System.Threading.Tasks;

namespace InvestmentSimulator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SimulationController : ControllerBase
    {
        private readonly ITraditionalSimulatorService _traditionalSimulationService;
        private readonly ICryptoSimulatorService _cryptoSimulationService;
        private readonly IValidator<TraditionalSimulationInput> _validator;

        public SimulationController(
            ITraditionalSimulatorService traditionalSimulationService,
            ICryptoSimulatorService cryptoSimulationService,
            IValidator<TraditionalSimulationInput> validator)
        {
            _traditionalSimulationService = traditionalSimulationService;
            _cryptoSimulationService = cryptoSimulationService;
            _validator = validator;
        }

        [HttpPost("simulate")]
        public IActionResult Simulate([FromBody] TraditionalSimulationInput investment)
        {
            var validationResult = _validator.Validate(investment);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            var result = _traditionalSimulationService.Simulate(investment);
            return Ok(result);
        }

        [HttpPost("crypto")]
        public async Task<IActionResult> SimulateCrypto([FromBody] CryptoSimulationInput investment)
        {
            var result = await _cryptoSimulationService.SimulateCryptoInvestmentAsync(investment);
            return Ok(result);
        }
    }
}