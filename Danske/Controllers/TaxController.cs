using Danske.Application.DTOs;
using Danske.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Danske.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxController : ControllerBase
    {
        private readonly ITaxService _taxService;

        public TaxController(ITaxService taxService)
        {
            _taxService = taxService;
        }

        /// <summary>
        /// Add or update tax records of municipality
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [SwaggerOperation(
            Summary = "Add or update municipality taxes",
            Description = "Provide the municipality name and tax information"
        )]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateTaxesDto dto)
        {
            await _taxService.AddUpdateTaxRecordsAsync(dto);

            return Ok();
        }

        /// <summary>
        /// Delete tax record by tax ID
        /// </summary>
        /// <param name="taxId"></param>
        /// <returns></returns>
        [SwaggerOperation(
            Summary = "Delete a tax record",
            Description = "Provide the tax ID"
        )]
        [HttpDelete("{taxId}")]
        public async Task<IActionResult> Delete(int taxId)
        {
            await _taxService.RemoveTaxByIdAsync(taxId);

            return Ok();
        }
    }
}
