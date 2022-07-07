using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("staging_TaskGroupTask")]
    [Index("TaskGroupId", Name = "IX_Staging_TaskGroupTask_TaskGroupID")]
    [Index("TaskId", Name = "IX_Staging_TaskGroupTask_TaskID")]
    public partial class StagingTaskGroupTask
    {
        [Key]
        [Column("TaskGroupTaskID")]
        public int TaskGroupTaskId { get; set; }
        [Column("TaskGroupID")]
        public int TaskGroupId { get; set; }
        [Column("TaskID")]
        public int TaskId { get; set; }

        [ForeignKey("TaskId")]
        [InverseProperty("StagingTaskGroupTasks")]
        public virtual StagingTask Task { get; set; } = null!;
        [ForeignKey("TaskGroupId")]
        [InverseProperty("StagingTaskGroupTasks")]
        public virtual StagingTaskGroup TaskGroup { get; set; } = null!;
    }
}
