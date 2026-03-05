using Danske.Application.DTOs;
using Danske.Domain.Aggregates.Tax;

namespace Danske.Application.Interfaces
{
    public interface IMunicipalityService
    {
        Task<IEnumerable<MunicipalityDto>> GetExistingMunicipalities(string? municipalityName, TaxType? taxType);

        Task<MunicipalityDto> GetMunicipalityByName(string municipalityName);

        Task<MunicipalityDto> AddMunicipalityAsync(CreateMunicipalityDto dto);

        Task DeleteMunicipality(string municipalityName);
    }
}
