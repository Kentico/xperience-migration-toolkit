using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_MetaFile")]
    [Index("MetaFileGuid", "MetaFileSiteId", "MetaFileObjectType", "MetaFileObjectId", "MetaFileGroupName", Name = "IX_CMS_MetaFile_MetaFileGUID_MetaFileSiteID_MetaFileObjectType_MetaFileObjectID_MetaFileGroupName")]
    [Index("MetaFileSiteId", Name = "IX_CMS_MetaFile_MetaFileSiteID")]
    public partial class CmsMetaFile
    {
        [Key]
        [Column("MetaFileID")]
        public int MetaFileId { get; set; }
        [Column("MetaFileObjectID")]
        public int MetaFileObjectId { get; set; }
        [StringLength(100)]
        public string MetaFileObjectType { get; set; } = null!;
        [StringLength(100)]
        public string? MetaFileGroupName { get; set; }
        [StringLength(250)]
        public string MetaFileName { get; set; } = null!;
        [StringLength(50)]
        public string MetaFileExtension { get; set; } = null!;
        public int MetaFileSize { get; set; }
        [StringLength(100)]
        public string MetaFileMimeType { get; set; } = null!;
        public byte[]? MetaFileBinary { get; set; }
        public int? MetaFileImageWidth { get; set; }
        public int? MetaFileImageHeight { get; set; }
        [Column("MetaFileGUID")]
        public Guid MetaFileGuid { get; set; }
        public DateTime MetaFileLastModified { get; set; }
        [Column("MetaFileSiteID")]
        public int? MetaFileSiteId { get; set; }
        [StringLength(250)]
        public string? MetaFileTitle { get; set; }
        public string? MetaFileDescription { get; set; }
        public string? MetaFileCustomData { get; set; }

        [ForeignKey("MetaFileSiteId")]
        [InverseProperty("CmsMetaFiles")]
        public virtual CmsSite? MetaFileSite { get; set; }
    }
}
