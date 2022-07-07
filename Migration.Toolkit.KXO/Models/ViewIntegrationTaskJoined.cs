using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Keyless]
    public partial class ViewIntegrationTaskJoined
    {
        [Column("SynchronizationID")]
        public int? SynchronizationId { get; set; }
        [Column("SynchronizationTaskID")]
        public int? SynchronizationTaskId { get; set; }
        [Column("SynchronizationConnectorID")]
        public int? SynchronizationConnectorId { get; set; }
        public DateTime? SynchronizationLastRun { get; set; }
        public string? SynchronizationErrorMessage { get; set; }
        public bool? SynchronizationIsRunning { get; set; }
        [Column("TaskID")]
        public int TaskId { get; set; }
        [Column("TaskNodeID")]
        public int? TaskNodeId { get; set; }
        [Column("TaskDocumentID")]
        public int? TaskDocumentId { get; set; }
        [StringLength(450)]
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
    }
}
