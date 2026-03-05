using Danske.Application.DTOs;
using Danske.Application.Interfaces;
using Danske.Domain.Aggregates.Tax;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Danske.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MunicipalityController : ControllerBase
    {
        private readonly IMunicipalityService _municipalityService;

        public MunicipalityController(IMunicipalityService municipalityService)
        {
            _municipalityService = municipalityService;
        }

        /// <summary>
        /// Get all municipalities and their taxes
        /// </summary>
        /// <returns></returns>
        [SwaggerOperation(
            Summary = "Get all municipalities and their taxes",
            Description = "Query with municipalityName and/or tax type. Provide nothing to see the full list"
        )]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? municipalityName = null, [FromQuery] TaxType? taxType = null)
        {
            var result = await _municipalityService.GetExistingMunicipalities(municipalityName, taxType);

            return Ok(result);
        }

        /// <summary>
        /// Get municipality and its taxes with municipality name
        /// </summary>
        /// <param name="municipalityName"></param>
        /// <returns></returns>
        [SwaggerOperation(
            Summary = "Get municipality and its taxes with municipality name",
            Description = "Provide municipality name"
        )]
        [HttpGet("{municipalityName}")]
        public async Task<IActionResult> Get(string municipalityName)
        {
            var result = await _municipalityService.GetMunicipalityByName(municipalityName);

            return Ok(result);
        }

        /// <summary>
        /// Create new municipality
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [SwaggerOperation(
            Summary = "Create new municipality",
            Description = "Provide municipality name"
        )]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateMunicipalityDto dto)
        {
            var result = await _municipalityService.AddMunicipalityAsync(dto);

            return Ok(result);
        }

        /// <summary>
        /// Delete municipality
        /// </summary>
        /// <param name="municipalityName"></param>
        /// <returns></returns>
        [SwaggerOperation(
            Summary = "Delete municipality",
            Description = "Provide municipality name"
        )]
        [HttpDelete("{municipalityName}")]
        public async Task<IActionResult> Delete(string municipalityName)
        {
            await _municipalityService.DeleteMunicipality(municipalityName);

            return Ok();
        }
    }
}
