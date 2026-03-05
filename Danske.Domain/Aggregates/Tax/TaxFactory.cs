
using Danske.Domain.Exceptions;
using System.Net;

namespace Danske.Domain.Aggregates.Tax
{
    public static class TaxFactory
    {
        public static Tax Create(int municipalityId, TaxType taxType, decimal rate, DateOnly startDate)
        {
            ValidateStartDate(taxType, startDate);

            return new Tax
            {
                MunicipalityId = municipalityId,
                TaxType = taxType,
                Rate = rate,
                StartDate = startDate,
                EndDate = CalculateEndDate(startDate, taxType)
            };
        }

        private static void ValidateStartDate(TaxType taxType, DateOnly startDate)
        {
            switch (taxType)
            {
                case TaxType.Monthly when startDate.Day != 1:
                    throw new BusinessException("monthly tax must start on the first day of the month", HttpStatusCode.BadRequest);

                case TaxType.Yearly when (startDate.Month != 1 || startDate.Day != 1):
                    throw new BusinessException("yearly tax must start on january 1", HttpStatusCode.BadRequest);
            }
        }

        private static DateOnly CalculateEndDate(DateOnly startDate, TaxType taxType) =>
            taxType switch
            {
                TaxType.Daily => startDate.AddDays(1),
                TaxType.Weekly => startDate.AddDays(7),
                TaxType.Monthly => startDate.AddMonths(1),
                TaxType.Yearly => startDate.AddYears(1),
                _ => throw new BusinessException("Unsupported tax type.", HttpStatusCode.BadRequest)
            };
    }
}
