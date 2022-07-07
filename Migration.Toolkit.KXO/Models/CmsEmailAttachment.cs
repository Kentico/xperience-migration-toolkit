using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_EmailAttachment")]
    public partial class CmsEmailAttachment
    {
        public CmsEmailAttachment()
        {
            Emails = new HashSet<CmsEmail>();
        }

        [Key]
        [Column("AttachmentID")]
        public int AttachmentId { get; set; }
        [StringLength(255)]
        public string AttachmentName { get; set; } = null!;
        [StringLength(50)]
        public string AttachmentExtension { get; set; } = null!;
        public int AttachmentSize { get; set; }
        [StringLength(100)]
        public string AttachmentMimeType { get; set; } = null!;
        public byte[] AttachmentBinary { get; set; } = null!;
        [Column("AttachmentGUID")]
        public Guid AttachmentGuid { get; set; }
        public DateTime AttachmentLastModified { get; set; }
        [Column("AttachmentContentID")]
        [StringLength(255)]
        public string? AttachmentContentId { get; set; }
        [Column("AttachmentSiteID")]
        public int? AttachmentSiteId { get; set; }

        [ForeignKey("AttachmentId")]
        [InverseProperty("Attachments")]
        public virtual ICollection<CmsEmail> Emails { get; set; }
    }
}
