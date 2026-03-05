using Danske.Application.DTOs;
using Danske.Application.Interfaces;
using Danske.Domain.Aggregates.Municipality;
using Danske.Domain.Aggregates.Tax;

namespace Danske.Application.Services.TaxStrategy
{
    public class YearlyTaxStrategy : ITaxStrategy
    {
        public TaxType TaxType => TaxType.Yearly;

        public void Handle(Municipality municipality, CreateTaxDto taxDto)
        {
            var newTax = TaxFactory.Create(
                municipality.Id,
                taxDto.TaxType,
                taxDto.Rate,
                taxDto.StartDate);

            var existing = municipality.Taxes?
                .Where(x => x.TaxType == TaxType.Yearly)
                .FirstOrDefault(x =>
                    x.StartDate.Year == newTax.StartDate.Year);

            if (existing != null)
            {
                existing.Rate = newTax.Rate;
                return;
            }

            municipality.Taxes?.Add(newTax);
        }
    }
}
