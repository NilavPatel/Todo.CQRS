using System;

namespace Framework.Snapshotting
{
    public abstract class Snapshot
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
    }
}