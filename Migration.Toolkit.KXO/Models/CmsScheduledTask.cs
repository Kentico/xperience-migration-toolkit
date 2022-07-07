using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_ScheduledTask")]
    [Index("TaskNextRunTime", "TaskEnabled", "TaskServerName", Name = "IX_CMS_ScheduledTask_TaskNextRunTime_TaskEnabled_TaskServerName")]
    [Index("TaskResourceId", Name = "IX_CMS_ScheduledTask_TaskResourceID")]
    [Index("TaskSiteId", "TaskDisplayName", Name = "IX_CMS_ScheduledTask_TaskSiteID_TaskDisplayName")]
    [Index("TaskUserId", Name = "IX_CMS_ScheduledTask_TaskUserID")]
    public partial class CmsScheduledTask
    {
        [Key]
        [Column("TaskID")]
        public int TaskId { get; set; }
        [StringLength(200)]
        public string TaskName { get; set; } = null!;
        [StringLength(200)]
        public string TaskDisplayName { get; set; } = null!;
        [StringLength(200)]
        public string TaskAssemblyName { get; set; } = null!;
        [StringLength(200)]
        public string? TaskClass { get; set; }
        [StringLength(1000)]
        public string TaskInterval { get; set; } = null!;
        public string TaskData { get; set; } = null!;
        public DateTime? TaskLastRunTime { get; set; }
        public DateTime? TaskNextRunTime { get; set; }
        public string? TaskLastResult { get; set; }
        [Column("TaskSiteID")]
        public int? TaskSiteId { get; set; }
        public bool? TaskDeleteAfterLastRun { get; set; }
        [StringLength(100)]
        public string? TaskServerName { get; set; }
        [Column("TaskGUID")]
        public Guid TaskGuid { get; set; }
        public DateTime TaskLastModified { get; set; }
        public int? TaskExecutions { get; set; }
        [Column("TaskResourceID")]
        public int? TaskResourceId { get; set; }
        public bool? TaskRunInSeparateThread { get; set; }
        public DateTime? TaskLastExecutionReset { get; set; }
        [StringLength(400)]
        public string? TaskCondition { get; set; }
        public bool? TaskRunIndividually { get; set; }
        [Column("TaskUserID")]
        public int? TaskUserId { get; set; }
        public int? TaskType { get; set; }
        [StringLength(100)]
        public string? TaskObjectType { get; set; }
        [Column("TaskObjectID")]
        public int? TaskObjectId { get; set; }
        [StringLength(200)]
        public string? TaskExecutingServerName { get; set; }
        public bool TaskEnabled { get; set; }
        public bool TaskIsRunning { get; set; }

        [ForeignKey("TaskResourceId")]
        [InverseProperty("CmsScheduledTasks")]
        public virtual CmsResource? TaskResource { get; set; }
        [ForeignKey("TaskSiteId")]
        [InverseProperty("CmsScheduledTasks")]
        public virtual CmsSite? TaskSite { get; set; }
        [ForeignKey("TaskUserId")]
        [InverseProperty("CmsScheduledTasks")]
        public virtual CmsUser? TaskUser { get; set; }
    }
}
