using System;

namespace Framework.CheckpointStore
{
    public class Checkpoint
    {
        public string SubscriptionId { get; set; }
        public long Position { get; set; }
    }
}