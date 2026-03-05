
namespace Danske.Application.DTOs
{
    public record TaxRateResult
    {
        public required string Municipality { get; init; }

        public required DateOnly Date { get; init; }

        public required decimal Rate { get; init; }

        public required string TaxType { get; init; }
    }
}
