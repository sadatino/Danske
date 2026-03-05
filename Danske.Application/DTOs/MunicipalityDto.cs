
namespace Danske.Application.DTOs
{
    public class MunicipalityDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<TaxDto> Taxes { get; set; } = new List<TaxDto>();
    }
}
