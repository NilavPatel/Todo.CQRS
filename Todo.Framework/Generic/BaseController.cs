
using System;
using Microsoft.AspNetCore.Mvc;
using Todo.Framework.CommandBus;

namespace Todo.Framework.Generic
{
    public class BaseController : Controller
    {
        public ICommandBus _bus;
        public BaseController(ICommandBus bus)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }
    }
}