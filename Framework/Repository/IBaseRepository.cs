using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Framework.Repository
{
    public interface IBaseRepository<TContext, T> : IReadRepository<TContext, T> where TContext : DbContext where T : BaseEntity
    {
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
