using System;
using System.Linq.Expressions;

namespace Todo.Framework.Core.Aggregate
{
    internal static class AggregateFactory<T>
    {
        private static readonly Func<T> _constructor = CreateTypeConstructor();

        private static Func<T> CreateTypeConstructor()
        {
            try
            {
                var newExpr = Expression.New(typeof(T));
                var func = Expression.Lambda<Func<T>>(newExpr);
                return func.Compile();
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        public static T CreateAggregate()
        {
            if (_constructor == null)
                throw new Exception($"{typeof(T).FullName} has no constructor without parameters. This can be either public or private");
            return _constructor();
        }
    }
}