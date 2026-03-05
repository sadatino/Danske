using Danske.Domain.Aggregates.Tax;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Danske.Application.DTOs
{
    public class CreateTaxDto
    {
        [Range(0, 3)]
        public TaxType TaxType { get; set; }

        [Range(0, 1)]
        public decimal Rate { get; set; }

        [Required]
        public DateOnly StartDate { get; set; }

        [JsonIgnore]
        public DateOnly EndDate { get; set; }
    }
}
