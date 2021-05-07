using System.Net;

namespace Framework.CommandBus
{
    public interface ICommandResult
    {
        HttpStatusCode StatusCode { get; set; }
        object Data { get; set; }
    }
}
