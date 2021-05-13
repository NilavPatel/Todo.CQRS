using System;
using System.ComponentModel.DataAnnotations;

namespace Framework.EventStore
{
    public class SnapshotEntity
    {
        [Key]
        public Guid SnapshotId { get; set; }
        public Guid AggregateId { get; set; }
        public int AggregateVersion { get; set; }
        public string SnapshotName { get; set; }
        public byte[] Data { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}