using Danske.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Danske.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxRateController : ControllerBase
    {
        private readonly ITaxRateService _taxRateService;

        public TaxRateController(ITaxRateService taxRateService)
        {
            _taxRateService = taxRateService;
        }

        /// <summary>
        /// Get applicable tax for municipality on specific date
        /// </summary>
        /// <param name="municipalityName"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [SwaggerOperation(
            Summary = "Get applicable tax for municipality on specific date",
            Description = "Provide the municipality name and date to retrieve the tax rate"
        )]
        [HttpGet("{municipalityName}")]
        public async Task<IActionResult> Get(string municipalityName, [FromQuery] DateOnly date)
        {
            var result = await _taxRateService.GetApplicableTaxRateAsync(municipalityName, date);

            return Ok(result);
        }
    }
}
