using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_LicenseKey")]
    public partial class CmsLicenseKey
    {
        [Key]
        [Column("LicenseKeyID")]
        public int LicenseKeyId { get; set; }
        [StringLength(255)]
        public string? LicenseDomain { get; set; }
        public string? LicenseKey { get; set; }
        [StringLength(200)]
        public string? LicenseEdition { get; set; }
        [StringLength(200)]
        public string? LicenseExpiration { get; set; }
        public int? LicenseServers { get; set; }
    }
}
