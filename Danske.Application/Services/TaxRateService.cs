using Danske.Application.DTOs;
using Danske.Application.Interfaces;
using Danske.Domain.Exceptions;
using Danske.Domain.Interfaces.Repositories;
using System.Net;

namespace Danske.Application.Services
{
    public class TaxRateService : ITaxRateService
    {
        private readonly IMunicipalityRepository _municipalityRepository;

        public TaxRateService(IMunicipalityRepository menicipalityRepository)
        {
            _municipalityRepository = menicipalityRepository;
        }

        public async Task<TaxRateResult> GetApplicableTaxRateAsync(string municipalityName, DateOnly date)
        {
            var municipality = await _municipalityRepository.GetMunicipalityByNameAsync(municipalityName, false);

            if (municipality == null)
            {
                throw new BusinessException($"no such municipality '{municipalityName}' exists", HttpStatusCode.NotFound);
            }

            var applicableTaxes = municipality.Taxes?
                .Where(x => x.StartDate <= date && x.EndDate > date)
                .ToList();

            if (applicableTaxes?.Count == 0)
            {
                throw new BusinessException($"no taxes applicable for '{municipalityName}' on {date}", HttpStatusCode.NotFound);
            }

            var appliedTax = applicableTaxes!
                .OrderBy(x => x.TaxType)
                .First();

            return new TaxRateResult
            {
                Municipality = municipality.Name,
                Date = date,
                Rate = appliedTax.Rate,
                TaxType = appliedTax.TaxType.ToString()
            };
        }
    }
}
