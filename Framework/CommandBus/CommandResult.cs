using System;
using System.Net;
using System.Collections.Generic;

namespace Framework.CommandBus
{
    public class CommandResult : ICommandResult
    {
        public CommandResult(HttpStatusCode statusCode, object data)
        {
            this.StatusCode = statusCode;
            this.Data = data;
        }

        public HttpStatusCode StatusCode { get; set; }
        public object Data { get; set; }
    }
}