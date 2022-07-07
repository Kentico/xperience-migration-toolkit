using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_ResourceLibrary")]
    [Index("ResourceLibraryResourceId", Name = "IX_CMS_ResourceLibrary")]
    public partial class CmsResourceLibrary
    {
        [Key]
        [Column("ResourceLibraryID")]
        public int ResourceLibraryId { get; set; }
        [Column("ResourceLibraryResourceID")]
        public int ResourceLibraryResourceId { get; set; }
        [StringLength(200)]
        public string ResourceLibraryPath { get; set; } = null!;

        [ForeignKey("ResourceLibraryResourceId")]
        [InverseProperty("CmsResourceLibraries")]
        public virtual CmsResource ResourceLibraryResource { get; set; } = null!;
    }
}
