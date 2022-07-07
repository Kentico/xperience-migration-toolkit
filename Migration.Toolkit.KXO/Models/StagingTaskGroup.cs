using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("staging_TaskGroup")]
    public partial class StagingTaskGroup
    {
        public StagingTaskGroup()
        {
            StagingTaskGroupTasks = new HashSet<StagingTaskGroupTask>();
            StagingTaskGroupUsers = new HashSet<StagingTaskGroupUser>();
        }

        [Key]
        [Column("TaskGroupID")]
        public int TaskGroupId { get; set; }
        [StringLength(50)]
        public string TaskGroupCodeName { get; set; } = null!;
        public Guid TaskGroupGuid { get; set; }
        public string? TaskGroupDescription { get; set; }

        [InverseProperty("TaskGroup")]
        public virtual ICollection<StagingTaskGroupTask> StagingTaskGroupTasks { get; set; }
        [InverseProperty("TaskGroup")]
        public virtual ICollection<StagingTaskGroupUser> StagingTaskGroupUsers { get; set; }
    }
}
