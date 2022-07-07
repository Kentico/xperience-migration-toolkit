using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_Query")]
    [Index("ClassId", "QueryName", Name = "IX_CMS_Query_QueryClassID_QueryName")]
    public partial class CmsQuery
    {
        [Key]
        [Column("QueryID")]
        public int QueryId { get; set; }
        [StringLength(100)]
        public string QueryName { get; set; } = null!;
        [Column("QueryTypeID")]
        public int QueryTypeId { get; set; }
        public string QueryText { get; set; } = null!;
        public bool QueryRequiresTransaction { get; set; }
        [Column("ClassID")]
        public int? ClassId { get; set; }
        public bool QueryIsLocked { get; set; }
        public DateTime QueryLastModified { get; set; }
        [Column("QueryGUID")]
        public Guid QueryGuid { get; set; }
        public bool? QueryIsCustom { get; set; }
        [StringLength(100)]
        public string? QueryConnectionString { get; set; }

        [ForeignKey("ClassId")]
        [InverseProperty("CmsQueries")]
        public virtual CmsClass? Class { get; set; }
    }
}
