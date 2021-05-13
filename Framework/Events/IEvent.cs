using System;

namespace Framework.Events
{
    public interface IEvent
    {
        Guid EventId { get; set; }
        Guid SourceId { get; set; }
        int Version { get; set; }
        DateTimeOffset OccuredOn { get; set; }
    }
}
