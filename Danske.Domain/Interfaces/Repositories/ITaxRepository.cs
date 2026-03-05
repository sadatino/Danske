using Danske.Domain.Aggregates.Tax;

namespace Danske.Domain.Interfaces.Repositories
{
    public interface ITaxRepository : IRepository<Tax>
    {
        Task<IEnumerable<Tax>> AddRangeAsync(IEnumerable<Tax> taxes);

        Task<Tax?> GetByIdAsync(int id, bool readOnly = true);
    }
}
