using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("Staging_TaskUser")]
    [Index("TaskId", Name = "IX_Staging_TaskUser_TaskID")]
    [Index("UserId", Name = "IX_Staging_TaskUser_UserID")]
    public partial class StagingTaskUser
    {
        [Key]
        [Column("TaskUserID")]
        public int TaskUserId { get; set; }
        [Column("TaskID")]
        public int TaskId { get; set; }
        [Column("UserID")]
        public int UserId { get; set; }

        [ForeignKey("TaskId")]
        [InverseProperty("StagingTaskUsers")]
        public virtual StagingTask Task { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("StagingTaskUsers")]
        public virtual CmsUser User { get; set; } = null!;
    }
}
