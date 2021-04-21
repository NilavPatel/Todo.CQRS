using System;
using System.Collections.Generic;
using System.Net;

namespace Todo.Framework.CommandBus
{
    public interface ICommandResult
    {
        HttpStatusCode StatusCode { get; set; }
        Guid? AggregateId { get; set; }
        int? AggregateVersion { get; set; }
        IEnumerable<object> Errors { get; set; }
    }
}
