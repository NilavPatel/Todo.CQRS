namespace Todo.Framework.Aggregate
{
    public class SaveAggregateEvents : Todo.Framework.Command.Command
    {
        public IAggregate Aggregate { get; set; }
    }
}
