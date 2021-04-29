using System;
using System.Linq;
using System.Reflection;
using Framework.Aggregate;

namespace Framework.Snapshotting
{
    public static class SnapshotStrategy
    {
        private const int _snapshotInterval = 10;

        public static bool IsSnapshotable(Type aggregateType)
        {
            while (true)
            {
                if (aggregateType.GetTypeInfo().BaseType == null) return false;
                if (aggregateType.GetTypeInfo().BaseType.GetTypeInfo().IsGenericType &&
                    aggregateType.GetTypeInfo().BaseType.GetGenericTypeDefinition() == typeof(SnapshotAggregateRoot<>))
                    return true;
                aggregateType = aggregateType.GetTypeInfo().BaseType;
            }
        }

        public static bool ShouldMakeSnapShot(IAggregateRoot aggregate)
        {
            if (!IsSnapshotable(aggregate.GetType()))
                return false;

            var i = aggregate.Version;
            for (var j = 0; j < aggregate.DomainEvents.Count(); j++)
                if (++i % _snapshotInterval == 0 && i != 0)
                    return true;
            return false;
        }

    }
}