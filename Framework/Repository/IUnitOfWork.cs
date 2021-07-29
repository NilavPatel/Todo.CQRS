using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Framework.Repository
{
    public interface IUnitOfWork<TContext> : IAsyncDisposable where TContext : DbContext
    {
        Task<int> SaveChangesAsync();
        IBaseRepository<TContext, T> Repository<T>() where T : BaseEntity;
    }
}