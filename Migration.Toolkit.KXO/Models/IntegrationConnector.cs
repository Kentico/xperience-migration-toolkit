using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("Integration_Connector")]
    [Index("ConnectorEnabled", Name = "IX_Integration_Connector_ConnectorEnabled")]
    public partial class IntegrationConnector
    {
        public IntegrationConnector()
        {
            IntegrationSynchronizations = new HashSet<IntegrationSynchronization>();
        }

        [Key]
        [Column("ConnectorID")]
        public int ConnectorId { get; set; }
        [StringLength(100)]
        public string ConnectorName { get; set; } = null!;
        [StringLength(440)]
        public string ConnectorDisplayName { get; set; } = null!;
        [StringLength(400)]
        public string ConnectorAssemblyName { get; set; } = null!;
        [StringLength(400)]
        public string ConnectorClassName { get; set; } = null!;
        [Required]
        public bool? ConnectorEnabled { get; set; }
        public DateTime ConnectorLastModified { get; set; }

        [InverseProperty("SynchronizationConnector")]
        public virtual ICollection<IntegrationSynchronization> IntegrationSynchronizations { get; set; }
    }
}
