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
        private readonly ITraditionalSimulationService _traditionalSimulationService;
        private readonly ICryptoSimulationService _cryptoSimulationService;
        private readonly IValidator<Investment> _validator;

        public SimulationController(
            ITraditionalSimulationService traditionalSimulationService,
            ICryptoSimulationService cryptoSimulationService,
            IValidator<Investment> validator)
        {
            _traditionalSimulationService = traditionalSimulationService;
            _cryptoSimulationService = cryptoSimulationService;
            _validator = validator;
        }

        [HttpPost("simulate")]
        public IActionResult Simulate([FromBody] Investment investment)
        {
            var validationResult = _validator.Validate(investment);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            var result = _traditionalSimulationService.Simulate(investment);
            return Ok(result);
        }

        [HttpPost("crypto")]
        public async Task<IActionResult> SimulateCrypto([FromBody] CryptoInvestment investment)
        {
            var result = await _cryptoSimulationService.SimulateCryptoInvestmentAsync(investment);
            return Ok(result);
        }
    }
}