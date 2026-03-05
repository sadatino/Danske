
namespace Danske.Domain.Aggregates.Tax
{
    public class Tax
    {
        public int Id { get; set; }

        public int MunicipalityId { get; set; }

        public Municipality.Municipality? Municipality { get; set; }

        public TaxType TaxType { get; set; }

        public decimal Rate { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }
    }
}
