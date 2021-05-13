using System;

namespace Framework.Events
{
    public class Event : IEvent
    {
        public Guid EventId { get; set; }
        public Guid SourceId { get; set; }
        public int Version { get; set; }
        public DateTimeOffset OccuredOn { get; set; }
    }
}
