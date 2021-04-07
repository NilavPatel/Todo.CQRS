
using Microsoft.AspNetCore.Mvc;
using Todo.Framework.Core.CommandBus;

namespace Todo.Framework.Core
{
    public class BaseController : Controller
    {
        public ICommandBus _bus;
        public BaseController(ICommandBus bus)
        {
            _bus = bus;
        }
    }
}