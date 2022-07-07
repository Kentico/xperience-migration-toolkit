using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("Staging_Synchronization")]
    [Index("SynchronizationServerId", Name = "IX_Staging_Synchronization_SynchronizationServerID")]
    [Index("SynchronizationTaskId", Name = "IX_Staging_Synchronization_SynchronizationTaskID")]
    public partial class StagingSynchronization
    {
        [Key]
        [Column("SynchronizationID")]
        public int SynchronizationId { get; set; }
        [Column("SynchronizationTaskID")]
        public int SynchronizationTaskId { get; set; }
        [Column("SynchronizationServerID")]
        public int SynchronizationServerId { get; set; }
        public DateTime? SynchronizationLastRun { get; set; }
        public string? SynchronizationErrorMessage { get; set; }

        [ForeignKey("SynchronizationServerId")]
        [InverseProperty("StagingSynchronizations")]
        public virtual StagingServer SynchronizationServer { get; set; } = null!;
        [ForeignKey("SynchronizationTaskId")]
        [InverseProperty("StagingSynchronizations")]
        public virtual StagingTask SynchronizationTask { get; set; } = null!;
    }
}
