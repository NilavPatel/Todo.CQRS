using System;
using System.ComponentModel.DataAnnotations;

namespace Framework.EventStore
{
    public class EventEntity
    {
        [Key]
        public Guid EventId { get; set; }
        public Guid AggregateId { get; set; }
        public int AggregateVersion { get; set; }
        public string AggregateName { get; set; }
        public string EventName { get; set; }
        public byte[] Data { get; set; }
        public DateTimeOffset OccuredOn { get; set; }
        public bool Success { get; set; }
    }
}