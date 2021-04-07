using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Todo.Framework.Core.Repository
{
    public interface IBaseRepository<TContext, T> where TContext : DbContext where T : BaseEntity
    {
        T GetById(Guid id);
        IReadOnlyList<T> ListAll();
        IReadOnlyList<T> List(ISpecification<T> spec);
        T FirstOrDefault(ISpecification<T> spec);
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        int Count(ISpecification<T> spec);
    }
}
