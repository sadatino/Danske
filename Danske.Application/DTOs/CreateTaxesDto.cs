
namespace Danske.Application.DTOs
{
    public record CreateTaxesDto
    {
        public required string MunicipalityName {  get; init; }

        public required IEnumerable<CreateTaxDto> Taxes { get; init; }
    }
}
