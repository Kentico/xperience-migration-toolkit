using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("Export_Task")]
    [Index("TaskSiteId", "TaskObjectType", Name = "IX_Export_Task_TaskSiteID_TaskObjectType")]
    public partial class ExportTask
    {
        [Key]
        [Column("TaskID")]
        public int TaskId { get; set; }
        [Column("TaskSiteID")]
        public int? TaskSiteId { get; set; }
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

        [ForeignKey("TaskSiteId")]
        [InverseProperty("ExportTasks")]
        public virtual CmsSite? TaskSite { get; set; }
    }
}
