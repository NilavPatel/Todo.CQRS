using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Repository
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
    {
        private TContext _dbContext;
        private readonly IServiceProvider _serviceProvider;
        private IDictionary<Type, dynamic> _repositories;

        public UnitOfWork(TContext dbContext, IServiceProvider serviceProvider)
        {
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this._serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this._repositories = new Dictionary<Type, dynamic>();
        }

        public IBaseRepository<TContext, T> Repository<T>() where T : BaseEntity
        {
            var entityType = typeof(T);
            if (this._repositories.ContainsKey(entityType))
            {
                return _repositories[entityType];
            }

            var repositoryType = typeof(BaseRepository<,>);
            var repository = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _dbContext);

            _repositories.Add(entityType, repository);
            return (IBaseRepository<TContext, T>)repository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await this._dbContext.SaveChangesAsync();
        }
    }
}