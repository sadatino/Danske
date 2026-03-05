using Danske.Domain.Interfaces.Repositories;
using Danske.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Danske.Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly DanskeDbContext _dbContext;

        public Repository(DanskeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity cant be null");
            }

            try
            {
                await _dbContext.AddAsync(entity);

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }
        }

        public void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(Delete)} entity cant be null");
            }

            try
            {
                _dbContext.Remove(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be deleted: {ex.Message}");
            }
        }

        protected IQueryable<TEntity> GetAll(bool readOnly = false, params Expression<Func<TEntity, object>>[] includes)
        {
            try
            {
                var query = _dbContext.Set<TEntity>().AsQueryable();

                if (readOnly)
                {
                    query = query.AsNoTracking();
                }

                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                return query;

            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }

        public TEntity Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(Update)} cant be null");
            }

            try
            {
                _dbContext.Update(entity);

                return entity;
            }
            catch(Exception ex)
            {
                throw new Exception($"failed to update {nameof(entity)}: {ex.Message}");
            }
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
