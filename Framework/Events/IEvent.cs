using System;

namespace Framework.Events
{
    public interface IEvent
    {
        Guid SourceId { get; set; }
        int Version { get; set; }
        DateTimeOffset OccuredOn { get; set; }
    }
}
