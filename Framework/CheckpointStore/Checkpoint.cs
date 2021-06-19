using System.ComponentModel.DataAnnotations;

namespace Framework.CheckpointStore
{
    public class Checkpoint
    {
        [Key]
        public string Module { get; set; }
        public long Commit { get; set; }
        public long Prepare { get; set; }
    }
}