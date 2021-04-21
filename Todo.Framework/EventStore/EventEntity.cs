using System;

namespace Todo.Framework.EventStore
{
    public class EventEntity
    {
        public Guid Id { get; set; }
        public string AggregateName { get; set; }
        public Guid AggregateId { get; set; }
        public int AggregateVersion { get; set; }
        public byte[] Data { get; set; }
        public string EventName { get; set; }
        public byte[] EventFullName { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}