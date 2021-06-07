using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Framework.Repository
{
    public class BaseRepository<TContext, T> : ReadRepository<TContext, T>, IBaseRepository<TContext, T> where TContext : DbContext where T : BaseEntity, new()
    {
        private readonly TContext _dbContext;

        public BaseRepository(TContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
