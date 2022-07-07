using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("staging_TaskGroupUser")]
    [Index("TaskGroupId", Name = "IX_Staging_TaskGroupUser_TaskGroup_ID")]
    [Index("UserId", Name = "IX_Staging_TaskGroupUser_UserID", IsUnique = true)]
    public partial class StagingTaskGroupUser
    {
        [Key]
        [Column("TaskGroupUserID")]
        public int TaskGroupUserId { get; set; }
        [Column("TaskGroupID")]
        public int TaskGroupId { get; set; }
        [Column("UserID")]
        public int UserId { get; set; }

        [ForeignKey("TaskGroupId")]
        [InverseProperty("StagingTaskGroupUsers")]
        public virtual StagingTaskGroup TaskGroup { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("StagingTaskGroupUser")]
        public virtual CmsUser User { get; set; } = null!;
    }
}
