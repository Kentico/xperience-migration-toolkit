using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("Export_History")]
    [Index("ExportSiteId", Name = "IX_Export_History_ExportSiteID")]
    [Index("ExportUserId", Name = "IX_Export_History_ExportUserID")]
    public partial class ExportHistory
    {
        [Key]
        [Column("ExportID")]
        public int ExportId { get; set; }
        public DateTime ExportDateTime { get; set; }
        [StringLength(450)]
        public string ExportFileName { get; set; } = null!;
        [Column("ExportSiteID")]
        public int? ExportSiteId { get; set; }
        [Column("ExportUserID")]
        public int? ExportUserId { get; set; }
        public string? ExportSettings { get; set; }

        [ForeignKey("ExportSiteId")]
        [InverseProperty("ExportHistories")]
        public virtual CmsSite? ExportSite { get; set; }
        [ForeignKey("ExportUserId")]
        [InverseProperty("ExportHistories")]
        public virtual CmsUser? ExportUser { get; set; }
    }
}
