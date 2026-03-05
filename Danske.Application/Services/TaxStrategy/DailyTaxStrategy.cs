using Danske.Application.DTOs;
using Danske.Application.Interfaces;
using Danske.Domain.Aggregates.Municipality;
using Danske.Domain.Aggregates.Tax;

namespace Danske.Application.Services.TaxStrategy
{
    public class DailyTaxStrategy : ITaxStrategy
    {
        public TaxType TaxType => TaxType.Daily;

        public void Handle(Municipality municipality, CreateTaxDto taxDto)
        {
            var newTax = TaxFactory.Create(
                municipality.Id,
                taxDto.TaxType,
                taxDto.Rate,
                taxDto.StartDate);

            var existing = municipality.Taxes?
                .FirstOrDefault(x =>
                    x.TaxType == TaxType.Daily &&
                    x.StartDate == newTax.StartDate &&
                    x.EndDate == newTax.EndDate);

            if (existing != null)
            {
                existing.Rate = newTax.Rate;
                return;
            }

            municipality.Taxes?.Add(newTax);
        }
    }
}
