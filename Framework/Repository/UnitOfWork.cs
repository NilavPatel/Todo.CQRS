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
        private bool _disposed = false;

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
            //TODO: Need to find a way to use dependency injection with passing existing dbcontext object
            var repository = new BaseRepository<TContext, T>(_dbContext);
            _repositories.Add(entityType, repository);
            return repository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await this._dbContext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (!this._disposed)
            {
                await this._dbContext.DisposeAsync();
                this._disposed = true;
            }
        }
    }
}