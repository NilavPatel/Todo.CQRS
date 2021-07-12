using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Framework.Repository
{
    public class BaseRepository<TContext, T> : ReadRepository<TContext, T>, IBaseRepository<TContext, T> where TContext : DbContext where T : BaseEntity
    {
        private readonly TContext _dbContext;

        public BaseRepository(TContext dbContext) : base(dbContext, false)
        {
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<T> AddAsync(T entity)
        {
            await this._dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public void Update(T entity)
        {
            this._dbContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            this._dbContext.Set<T>().Remove(entity);
        }
    }
}
