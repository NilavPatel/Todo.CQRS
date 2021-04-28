using System;
using System.Threading.Tasks;
using Framework.Aggregate;

namespace Framework.UnitOfWork
{
    public interface IUowRepository
    {
        Task Add<T>(T aggregate) where T : AggregateRoot;

        Task<T> Get<T>(Guid id, int? version = null) where T : AggregateRoot;

        Task Commit();
    }
}