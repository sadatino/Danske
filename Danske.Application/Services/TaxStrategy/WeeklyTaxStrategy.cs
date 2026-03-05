using Danske.Application.DTOs;
using Danske.Application.Interfaces;
using Danske.Domain.Aggregates.Municipality;
using Danske.Domain.Aggregates.Tax;
using Danske.Domain.Exceptions;
using System.Net;

namespace Danske.Application.Services.TaxStrategy
{
    public class WeeklyTaxStrategy : ITaxStrategy
    {
        public TaxType TaxType => TaxType.Weekly;

        public void Handle(Municipality municipality, CreateTaxDto taxDto)
        {
            var newTax = TaxFactory.Create(
                municipality.Id,
                taxDto.TaxType,
                taxDto.Rate,
                taxDto.StartDate);

            var weeklyTaxes = municipality.Taxes?
                .Where(x => x.TaxType == TaxType.Weekly);

            var overlapping = weeklyTaxes?.FirstOrDefault(x =>
                newTax.StartDate <= x.EndDate &&
                newTax.EndDate >= x.StartDate);

            if (overlapping != null)
            {
                if (overlapping.StartDate == newTax.StartDate &&
                    overlapping.EndDate == newTax.EndDate)
                {
                    overlapping.Rate = newTax.Rate;
                    return;
                }

                throw new BusinessException(
                    "weekly tax period overlaps an existing weekly tax",
                    HttpStatusCode.Conflict);
            }

            municipality.Taxes?.Add(newTax);
        }
    }
}
