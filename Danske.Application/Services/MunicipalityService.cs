using Danske.Application.DTOs;
using Danske.Application.Interfaces;
using Danske.Domain.Aggregates.Municipality;
using Danske.Domain.Aggregates.Tax;
using Danske.Domain.Exceptions;
using Danske.Domain.Interfaces.Repositories;
using System.Net;

namespace Danske.Application.Services
{
    public class MunicipalityService : IMunicipalityService
    {
        private readonly IMunicipalityRepository _municipalityRepository;

        public MunicipalityService(IMunicipalityRepository municipalityRepository)
        {
            _municipalityRepository = municipalityRepository;
        }

        public async Task<IEnumerable<MunicipalityDto>> GetExistingMunicipalities(string? municipalityName, TaxType? taxType)
        {
            var municipalities = await _municipalityRepository
                .GetExistingMunicipalitiesAsync(municipalityName, taxType);

            var result = municipalities.Select(x => new MunicipalityDto
            {
                Id = x.Id,
                Name = x.Name,
                Taxes = x.Taxes
                    .Where(t => !taxType.HasValue || t.TaxType == taxType.Value)
                    .Select(t => new TaxDto
                    {
                        Id = t.Id,
                        Rate = t.Rate,
                        TaxType = t.TaxType,
                        StartDate = t.StartDate,
                        EndDate = t.EndDate
                    })
                    .ToList()
            });

            return result;
        }

        public async Task<MunicipalityDto> GetMunicipalityByName(string municipalityName)
        {
            var m = await _municipalityRepository.GetMunicipalityByNameAsync(municipalityName);

            if (m == null)
            {
                throw new BusinessException($"no such municipality {municipalityName} exists", HttpStatusCode.NotFound);
            }

            var result = new MunicipalityDto
            {
                Id = m.Id,
                Name = m.Name,
                Taxes = m.Taxes?.Select(x => new TaxDto
                {
                    Id = x.Id,
                    TaxType = x.TaxType,
                    Rate = x.Rate,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate
                }).ToList() ?? []
            };

            return result;
        }

        public async Task<MunicipalityDto> AddMunicipalityAsync(CreateMunicipalityDto dto)
        {
            var municipalityExists = await _municipalityRepository.MunicipalityExistsByNameAsync(dto.Name);

            if (municipalityExists)
            {
                throw new BusinessException($"municipality {dto.Name} already exists", HttpStatusCode.Conflict);
            }

            var m = new Municipality
            {
                Name = dto.Name
            };

            await _municipalityRepository.AddAsync(m);
            await _municipalityRepository.SaveChangesAsync();

            return new MunicipalityDto
            {
                Id = m.Id,
                Name = m.Name
            };
        }

        public async Task DeleteMunicipality(string municipalityName)
        {
            var municipality = await _municipalityRepository.GetMunicipalityByNameAsync(municipalityName, false);

            if (municipality == null)
            {
                throw new BusinessException($"no such municipality {municipalityName} exists", HttpStatusCode.NotFound);
            }

            _municipalityRepository.Delete(municipality);
            await _municipalityRepository.SaveChangesAsync();
        }
    }
}
