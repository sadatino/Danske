using Danske.Domain.Aggregates.Municipality;
using Danske.Domain.Aggregates.Tax;

namespace Danske.Domain.Interfaces.Repositories
{
    public interface IMunicipalityRepository : IRepository<Municipality>
    {
        Task<IEnumerable<Municipality>> GetExistingMunicipalitiesAsync(
            string? municipalityName = null,
            TaxType? taxType = null);

        Task<Municipality?> GetMunicipalityByNameAsync(string municipalityName, bool readOnly = true);

        Task<bool> MunicipalityExistsByNameAsync(string municipalityName);
    }
}
