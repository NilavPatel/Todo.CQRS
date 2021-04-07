using Todo.Framework.Core.Aggregate;

namespace Todo.Framework.Core.Command
{
    public class SaveEvents : Command
    {
        public IAggregateRoot Aggregate { get; set; }
    }
}
