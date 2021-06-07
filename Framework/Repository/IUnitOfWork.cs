using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Framework.Repository
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        Task SaveChangesAsync();
        IBaseRepository<TContext, T> Repository<T>() where T : BaseEntity;
    }
}