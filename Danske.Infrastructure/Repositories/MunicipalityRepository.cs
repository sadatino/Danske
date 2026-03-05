using Danske.Domain.Aggregates.Municipality;
using Danske.Domain.Aggregates.Tax;
using Danske.Domain.Interfaces.Repositories;
using Danske.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Danske.Infrastructure.Repositories
{
    public class MunicipalityRepository : Repository<Municipality>, IMunicipalityRepository
    {
        public MunicipalityRepository(DanskeDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Municipality>> GetExistingMunicipalitiesAsync(
            string? municipalityName = null,
            TaxType? taxType = null)
        {
            var query = GetAll(true, x => x.Taxes);

            if (!string.IsNullOrWhiteSpace(municipalityName))
            {
                var lowerName = municipalityName.ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(lowerName));
            }

            if (taxType.HasValue)
            {
                query = query.Where(x => x.Taxes.Any(t => t.TaxType == taxType.Value));
            }

            return await query.ToListAsync();
        }

        public async Task<Municipality?> GetMunicipalityByNameAsync(string municipalityName, bool readOnly = true)
        {
            var municipality = await GetAll(readOnly, x => x.Taxes)
                .FirstOrDefaultAsync(x => x.Name.ToLower() == municipalityName.ToLower());

            return municipality;
        }

        public async Task<bool> MunicipalityExistsByNameAsync(string municipalityName)
        {
            return await GetAll(readOnly: true)
                .AnyAsync(x => x.Name.ToLower() == municipalityName.ToLower());
        }
    }
}
