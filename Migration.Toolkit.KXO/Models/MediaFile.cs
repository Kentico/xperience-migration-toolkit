using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("Media_File")]
    [Index("FileCreatedByUserId", Name = "IX_Media_File_FileCreatedByUserID")]
    [Index("FileLibraryId", Name = "IX_Media_File_FileLibraryID")]
    [Index("FileModifiedByUserId", Name = "IX_Media_File_FileModifiedByUserID")]
    [Index("FileSiteId", "FileGuid", Name = "IX_Media_File_FileSiteID_FileGUID")]
    public partial class MediaFile
    {
        [Key]
        [Column("FileID")]
        public int FileId { get; set; }
        [StringLength(250)]
        public string FileName { get; set; } = null!;
        [StringLength(250)]
        public string FileTitle { get; set; } = null!;
        public string FileDescription { get; set; } = null!;
        [StringLength(50)]
        public string FileExtension { get; set; } = null!;
        [StringLength(100)]
        public string FileMimeType { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public long FileSize { get; set; }
        public int? FileImageWidth { get; set; }
        public int? FileImageHeight { get; set; }
        [Column("FileGUID")]
        public Guid FileGuid { get; set; }
        [Column("FileLibraryID")]
        public int FileLibraryId { get; set; }
        [Column("FileSiteID")]
        public int FileSiteId { get; set; }
        [Column("FileCreatedByUserID")]
        public int? FileCreatedByUserId { get; set; }
        public DateTime FileCreatedWhen { get; set; }
        [Column("FileModifiedByUserID")]
        public int? FileModifiedByUserId { get; set; }
        public DateTime FileModifiedWhen { get; set; }
        public string? FileCustomData { get; set; }

        [ForeignKey("FileCreatedByUserId")]
        [InverseProperty("MediaFileFileCreatedByUsers")]
        public virtual CmsUser? FileCreatedByUser { get; set; }
        [ForeignKey("FileLibraryId")]
        [InverseProperty("MediaFiles")]
        public virtual MediaLibrary FileLibrary { get; set; } = null!;
        [ForeignKey("FileModifiedByUserId")]
        [InverseProperty("MediaFileFileModifiedByUsers")]
        public virtual CmsUser? FileModifiedByUser { get; set; }
        [ForeignKey("FileSiteId")]
        [InverseProperty("MediaFiles")]
        public virtual CmsSite FileSite { get; set; } = null!;
    }
}
