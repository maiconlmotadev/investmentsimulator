using Microsoft.AspNetCore.Mvc;
using InvestmentSimulator.Application.Services.Simulation.Deposit;
using InvestmentSimulator.Domain.Models.Simulation.Deposit;
using FluentValidation;
using System.Threading.Tasks;
using System;

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
            try
            {
                var validationResult = await _validator.ValidateAsync(investment);
                if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
    
                var result = await _depositSimulatorService.SimulateAsync(investment);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                // Captura erros que podem ocorrer ao buscar dados externos (ex: API do BCE offline).
                // Retorna um status 503 (Service Unavailable) que é mais apropriado para dependências externas.
                return StatusCode(503, new { message = "O serviço de simulação está temporariamente indisponível devido a uma falha na obtenção de dados de mercado. Por favor, tente novamente mais tarde.", details = ex.Message });
            }
        }
    }
}
