using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_WebFarmServer")]
    [Index("ServerName", Name = "IX_CMS_WebFarmServer_ServerName", IsUnique = true)]
    public partial class CmsWebFarmServer
    {
        public CmsWebFarmServer()
        {
            CmsWebFarmServerTasks = new HashSet<CmsWebFarmServerTask>();
        }

        [Key]
        [Column("ServerID")]
        public int ServerId { get; set; }
        [StringLength(300)]
        public string ServerDisplayName { get; set; } = null!;
        [StringLength(300)]
        public string ServerName { get; set; } = null!;
        [Column("ServerGUID")]
        public Guid? ServerGuid { get; set; }
        public DateTime ServerLastModified { get; set; }
        public bool ServerEnabled { get; set; }

        [InverseProperty("Server")]
        public virtual ICollection<CmsWebFarmServerTask> CmsWebFarmServerTasks { get; set; }
    }
}
