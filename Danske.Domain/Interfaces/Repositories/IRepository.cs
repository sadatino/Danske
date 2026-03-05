using System.Linq.Expressions;

namespace Danske.Domain.Interfaces.Repositories
{
    public interface IRepository<TEntity>
    {
        IQueryable<TEntity> GetAll(bool readOnly = false, params Expression<Func<TEntity, object>>[] includes);

        Task<TEntity> AddAsync(TEntity entity);

        TEntity Update(TEntity entity);

        void Delete(TEntity entity);

        Task SaveChangesAsync();
    }
}
