using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_WebFarmServerLog")]
    public partial class CmsWebFarmServerLog
    {
        [Key]
        [Column("WebFarmServerLogID")]
        public int WebFarmServerLogId { get; set; }
        public DateTime LogTime { get; set; }
        [StringLength(200)]
        public string LogCode { get; set; } = null!;
        [Column("ServerID")]
        public int ServerId { get; set; }
    }
}
