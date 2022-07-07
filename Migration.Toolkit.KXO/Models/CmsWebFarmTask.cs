using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_WebFarmTask")]
    [Index("TaskIsMemory", "TaskCreated", Name = "IX_CMS_WebFarmTask_TaskIsMemory_TaskCreated")]
    public partial class CmsWebFarmTask
    {
        public CmsWebFarmTask()
        {
            CmsWebFarmServerTasks = new HashSet<CmsWebFarmServerTask>();
        }

        [Key]
        [Column("TaskID")]
        public int TaskId { get; set; }
        [StringLength(100)]
        public string TaskType { get; set; } = null!;
        public string? TaskTextData { get; set; }
        public byte[]? TaskBinaryData { get; set; }
        public DateTime? TaskCreated { get; set; }
        public string? TaskTarget { get; set; }
        [StringLength(450)]
        public string? TaskMachineName { get; set; }
        [Column("TaskGUID")]
        public Guid? TaskGuid { get; set; }
        public bool? TaskIsAnonymous { get; set; }
        public string? TaskErrorMessage { get; set; }
        public bool? TaskIsMemory { get; set; }

        [InverseProperty("Task")]
        public virtual ICollection<CmsWebFarmServerTask> CmsWebFarmServerTasks { get; set; }
    }
}
