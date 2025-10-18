
using InvestmentSimulator.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InvestmentSimulator.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarketDataController : ControllerBase
    {
        private readonly IEcbDataService _ecbDataService;

        public MarketDataController(IEcbDataService ecbDataService)
        {
            _ecbDataService = ecbDataService;
        }

        [HttpGet("inflation")]
        public async Task<IActionResult> GetLatestInflationRate()
        {
            var rate = await _ecbDataService.GetLatestInflationRateAsync();
            return Ok(rate);
        }

        [HttpGet("deposit-rate-up-to-one-year")]
        public async Task<IActionResult> GetDepositRateUpToOneYear()
        {
            var rate = await _ecbDataService.GetDepositRateUpToOneYearAsync();
            return Ok(rate);
        }

        [HttpGet("deposit-rate-over-two-years")]
        public async Task<IActionResult> GetDepositRateOverTwoYears()
        {
            var rate = await _ecbDataService.GetDepositRateOverTwoYearsAsync();
            return Ok(rate);
        }

        [HttpGet("deposit-facility-rate")]
        public async Task<IActionResult> GetDepositFacilityRate()
        {
            var rate = await _ecbDataService.GetDepositFacilityRateAsync();
            return Ok(rate);
        }

        [HttpGet("ten-year-government-bond-yield")]
        public async Task<IActionResult> GetTenYearGovernmentBondYield()
        {
            var rate = await _ecbDataService.GetTenYearGovernmentBondYieldAsync();
            return Ok(rate);
        }
    }
}
