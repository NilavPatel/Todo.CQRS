using System;
using System.Threading.Tasks;
using Framework.Aggregate;

namespace Framework.Session
{
    public interface ISessionRepository
    {
        Task AddAsync<T>(T aggregate) where T : AggregateRoot;

        Task<T> GetAsync<T>(Guid id, int? version = null) where T : AggregateRoot;

        Task<bool> ExistAsync(Guid id);

        Task CommitAsync();
    }
}