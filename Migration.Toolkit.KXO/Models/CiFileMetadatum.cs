using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CI_FileMetadata")]
    [Index("FileLocation", Name = "UQ_CI_FileMetadata_FileLocation", IsUnique = true)]
    public partial class CiFileMetadatum
    {
        [Key]
        [Column("FileMetadataID")]
        public int FileMetadataId { get; set; }
        [StringLength(260)]
        public string FileLocation { get; set; } = null!;
        [StringLength(32)]
        public string FileHash { get; set; } = null!;
    }
}
