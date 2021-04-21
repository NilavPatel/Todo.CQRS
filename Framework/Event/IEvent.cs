using System;

namespace Framework.Event
{
    public interface IEvent
    {
        Guid SourceId { get; set; }
        int Version { get; set; }
        DateTimeOffset OccuredOn { get; set; }
    }
}
