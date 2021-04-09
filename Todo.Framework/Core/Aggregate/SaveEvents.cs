namespace Todo.Framework.Core.Aggregate
{
    public class SaveEvents : Todo.Framework.Core.Command.Command
    {
        public IAggregate Aggregate { get; set; }
    }
}
