namespace Framework.Aggregate
{
    public class SaveAggregateEvents : Framework.Command.Command
    {
        public IAggregate Aggregate { get; set; }
    }
}
