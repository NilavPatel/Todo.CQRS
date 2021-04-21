using System;
using System.Net;
using System.Collections.Generic;

namespace Framework.CommandBus
{
    public class CommandResult : ICommandResult
    {
        public CommandResult(HttpStatusCode statusCode, Guid? aggregateId, int? aggregateVersion, IEnumerable<object> errors)
        {
            this.StatusCode = statusCode;
            this.AggregateId = aggregateId;
            this.AggregateVersion = aggregateVersion;
            this.Errors = errors;
        }

        public HttpStatusCode StatusCode { get; set; }
        public Guid? AggregateId { get; set; }
        public int? AggregateVersion { get; set; }
        public IEnumerable<object> Errors { get; set; }
    }
}