using System;

namespace Framework.Event
{
    public class Event : IEvent
    {
        public Guid SourceId { get; set; }
        public int Version { get; set; }
        public DateTimeOffset OccuredOn { get; set; }
    }
}
