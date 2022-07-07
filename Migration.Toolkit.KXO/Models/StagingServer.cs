using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("Staging_Server")]
    [Index("ServerEnabled", Name = "IX_Staging_Server_ServerEnabled")]
    [Index("ServerSiteId", Name = "IX_Staging_Server_ServerSiteID")]
    public partial class StagingServer
    {
        public StagingServer()
        {
            StagingSynchronizations = new HashSet<StagingSynchronization>();
        }

        [Key]
        [Column("ServerID")]
        public int ServerId { get; set; }
        [StringLength(100)]
        public string ServerName { get; set; } = null!;
        [StringLength(440)]
        public string ServerDisplayName { get; set; } = null!;
        [Column("ServerSiteID")]
        public int ServerSiteId { get; set; }
        [Column("ServerURL")]
        [StringLength(450)]
        public string ServerUrl { get; set; } = null!;
        [Required]
        public bool? ServerEnabled { get; set; }
        [StringLength(20)]
        public string ServerAuthentication { get; set; } = null!;
        [StringLength(100)]
        public string? ServerUsername { get; set; }
        [StringLength(100)]
        public string? ServerPassword { get; set; }
        [Column("ServerX509ClientKeyID")]
        [StringLength(200)]
        public string? ServerX509clientKeyId { get; set; }
        [Column("ServerX509ServerKeyID")]
        [StringLength(200)]
        public string? ServerX509serverKeyId { get; set; }
        [Column("ServerGUID")]
        public Guid ServerGuid { get; set; }
        public DateTime ServerLastModified { get; set; }

        [ForeignKey("ServerSiteId")]
        [InverseProperty("StagingServers")]
        public virtual CmsSite ServerSite { get; set; } = null!;
        [InverseProperty("SynchronizationServer")]
        public virtual ICollection<StagingSynchronization> StagingSynchronizations { get; set; }
    }
}
