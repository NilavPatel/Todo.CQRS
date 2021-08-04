namespace Framework.BackgroundProcessor
{
    public interface IBackgroundEventProcessor
    {
        void Start(string subscriptionId);
    }
}