using System;
using System.Reflection;
using Framework.Aggregate;

namespace Framework.Snapshotting
{
    public static class SnapshotStrategy
    {
        private const int _snapshotInterval = 100;

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

        public static bool ShouldMakeSnapshot(IAggregateRoot aggregate)
        {
            if (!IsSnapshotable(aggregate.GetType()))
                return false;

            if (aggregate.Version != 0 && aggregate.Version % _snapshotInterval == 0)
                return true;
            return false;
        }
    }
}