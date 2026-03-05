using Danske.Domain.Aggregates.Tax;
using Danske.Domain.Interfaces.Repositories;
using Danske.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Danske.Infrastructure.Repositories
{
    public class TaxRepository : Repository<Tax>, ITaxRepository
    {
        public TaxRepository(DanskeDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Tax>> AddRangeAsync(IEnumerable<Tax> taxes)
        {
            await _dbContext.AddRangeAsync(taxes);

            return taxes;
        }

        public async Task<Tax?> GetByIdAsync(int id, bool readOnly = true)
        {
            return await GetAll(readOnly).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
