using Danske.Domain.Aggregates.Tax;

namespace Danske.Application.DTOs
{
    public class TaxDto
    {
        public int Id { get; set; }

        public TaxType TaxType { get; set; }

        public decimal Rate { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }
    }
}
