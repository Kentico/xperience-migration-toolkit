using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Keyless]
    public partial class ViewCmsUserDocument
    {
        [StringLength(100)]
        public string? DocumentName { get; set; }
        [Column("NodeSiteID")]
        public int NodeSiteId { get; set; }
        [Column("NodeID")]
        public int NodeId { get; set; }
        [StringLength(100)]
        public string ClassName { get; set; } = null!;
        [StringLength(100)]
        public string ClassDisplayName { get; set; } = null!;
        public DateTime? DocumentModifiedWhen { get; set; }
        [StringLength(50)]
        public string DocumentCulture { get; set; } = null!;
        [StringLength(200)]
        public string? CultureName { get; set; }
        [Column("UserID1")]
        public int? UserId1 { get; set; }
        [Column("UserID2")]
        public int? UserId2 { get; set; }
        [Column("UserID3")]
        public int? UserId3 { get; set; }
        [Column("DocumentWorkflowStepID")]
        public int? DocumentWorkflowStepId { get; set; }
        [StringLength(450)]
        public string NodeAliasPath { get; set; } = null!;
        [StringLength(12)]
        [Unicode(false)]
        public string Type { get; set; } = null!;
    }
}
