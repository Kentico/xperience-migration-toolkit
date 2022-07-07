using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("Staging_Task")]
    [Index("TaskDocumentId", "TaskNodeId", "TaskRunning", Name = "IX_Staging_Task_TaskDocumentID_TaskNodeID_TaskRunning")]
    [Index("TaskObjectType", "TaskObjectId", "TaskRunning", Name = "IX_Staging_Task_TaskObjectType_TaskObjectID_TaskRunning")]
    [Index("TaskSiteId", Name = "IX_Staging_Task_TaskSiteID")]
    [Index("TaskType", Name = "IX_Staging_Task_TaskType")]
    public partial class StagingTask
    {
        public StagingTask()
        {
            StagingSynchronizations = new HashSet<StagingSynchronization>();
            StagingTaskGroupTasks = new HashSet<StagingTaskGroupTask>();
            StagingTaskUsers = new HashSet<StagingTaskUser>();
        }

        [Key]
        [Column("TaskID")]
        public int TaskId { get; set; }
        [Column("TaskSiteID")]
        public int? TaskSiteId { get; set; }
        [Column("TaskDocumentID")]
        public int? TaskDocumentId { get; set; }
        [StringLength(450)]
        public string? TaskNodeAliasPath { get; set; }
        [StringLength(450)]
        public string TaskTitle { get; set; } = null!;
        public string TaskData { get; set; } = null!;
        public DateTime TaskTime { get; set; }
        [StringLength(50)]
        public string TaskType { get; set; } = null!;
        [StringLength(100)]
        public string? TaskObjectType { get; set; }
        [Column("TaskObjectID")]
        public int? TaskObjectId { get; set; }
        public bool? TaskRunning { get; set; }
        [Column("TaskNodeID")]
        public int? TaskNodeId { get; set; }
        public string? TaskServers { get; set; }

        [ForeignKey("TaskSiteId")]
        [InverseProperty("StagingTasks")]
        public virtual CmsSite? TaskSite { get; set; }
        [InverseProperty("SynchronizationTask")]
        public virtual ICollection<StagingSynchronization> StagingSynchronizations { get; set; }
        [InverseProperty("Task")]
        public virtual ICollection<StagingTaskGroupTask> StagingTaskGroupTasks { get; set; }
        [InverseProperty("Task")]
        public virtual ICollection<StagingTaskUser> StagingTaskUsers { get; set; }
    }
}
