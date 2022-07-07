using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("Integration_Task")]
    [Index("TaskIsInbound", Name = "IX_Integration_Task_TaskIsInbound")]
    [Index("TaskSiteId", Name = "IX_Integration_Task_TaskSiteID")]
    [Index("TaskType", Name = "IX_Integration_Task_TaskType")]
    public partial class IntegrationTask
    {
        public IntegrationTask()
        {
            IntegrationSynchronizations = new HashSet<IntegrationSynchronization>();
        }

        [Key]
        [Column("TaskID")]
        public int TaskId { get; set; }
        [Column("TaskNodeID")]
        public int? TaskNodeId { get; set; }
        [Column("TaskDocumentID")]
        public int? TaskDocumentId { get; set; }
        public string? TaskNodeAliasPath { get; set; }
        [StringLength(450)]
        public string TaskTitle { get; set; } = null!;
        public DateTime TaskTime { get; set; }
        [StringLength(50)]
        public string TaskType { get; set; } = null!;
        [StringLength(100)]
        public string? TaskObjectType { get; set; }
        [Column("TaskObjectID")]
        public int? TaskObjectId { get; set; }
        public bool TaskIsInbound { get; set; }
        [StringLength(50)]
        public string? TaskProcessType { get; set; }
        public string TaskData { get; set; } = null!;
        [Column("TaskSiteID")]
        public int? TaskSiteId { get; set; }
        [StringLength(50)]
        public string? TaskDataType { get; set; }

        [ForeignKey("TaskSiteId")]
        [InverseProperty("IntegrationTasks")]
        public virtual CmsSite? TaskSite { get; set; }
        [InverseProperty("SynchronizationTask")]
        public virtual ICollection<IntegrationSynchronization> IntegrationSynchronizations { get; set; }
    }
}
