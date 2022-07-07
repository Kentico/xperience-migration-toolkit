using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("OM_ContactStatus")]
    public partial class OmContactStatus
    {
        public OmContactStatus()
        {
            OmContacts = new HashSet<OmContact>();
        }

        [Key]
        [Column("ContactStatusID")]
        public int ContactStatusId { get; set; }
        [StringLength(200)]
        public string ContactStatusName { get; set; } = null!;
        [StringLength(200)]
        public string ContactStatusDisplayName { get; set; } = null!;
        public string? ContactStatusDescription { get; set; }

        [InverseProperty("ContactStatus")]
        public virtual ICollection<OmContact> OmContacts { get; set; }
    }
}
