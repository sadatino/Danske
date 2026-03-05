using System.Linq.Expressions;

namespace Danske.Domain.Interfaces.Repositories
{
    public interface IRepository<TEntity>
    {
        Task<TEntity> AddAsync(TEntity entity);

        TEntity Update(TEntity entity);

        void Delete(TEntity entity);

        Task SaveChangesAsync();
    }
}
