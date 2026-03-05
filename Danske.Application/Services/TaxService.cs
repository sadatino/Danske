using Danske.Application.DTOs;
using Danske.Application.Interfaces;
using Danske.Application.Services.TaxStrategy;
using Danske.Domain.Exceptions;
using Danske.Domain.Interfaces.Repositories;
using System.Net;

namespace Danske.Application.Services
{
    public class TaxService : ITaxService
    {
        private readonly ITaxRepository _taxRepository;
        private readonly IMunicipalityRepository _municipalityRepository;
        private readonly TaxStrategyResolver _taxStrategyResolver;

        public TaxService(ITaxRepository taxRepository,
            IMunicipalityRepository municipalityRepository,
            TaxStrategyResolver taxStrategyResolver)
        {
            _taxRepository = taxRepository;
            _municipalityRepository = municipalityRepository;
            _taxStrategyResolver = taxStrategyResolver;
        }

        public async Task AddUpdateTaxRecordsAsync(CreateTaxesDto dto)
        {
            var municipality = await _municipalityRepository.GetMunicipalityByNameAsync(dto.MunicipalityName, false);

            if (municipality is null)
            {
                throw new BusinessException($"no such municipality '{dto.MunicipalityName}' exists", HttpStatusCode.NotFound);
            }

            foreach (var taxDto in dto.Taxes)
            {
                var taxStrategy = _taxStrategyResolver.Resolve(taxDto.TaxType);
                taxStrategy.Handle(municipality, taxDto);
            }

            await _municipalityRepository.SaveChangesAsync();
        }

        public async Task RemoveTaxByIdAsync(int taxId)
        {
            var tax = await _taxRepository.GetByIdAsync(taxId, false);
            if (tax is null)
            {
                throw new BusinessException($"tax with ID: {taxId} not found", HttpStatusCode.NotFound);
            }

            _taxRepository.Delete(tax);
            await _taxRepository.SaveChangesAsync();
        }
    }
}
