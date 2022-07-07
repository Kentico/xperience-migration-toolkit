using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_FormFeaturedField")]
    public partial class CmsFormFeaturedField
    {
        [Key]
        [Column("FormFeaturedFieldID")]
        public int FormFeaturedFieldId { get; set; }
        public Guid FormFeaturedFieldGuid { get; set; }
        public DateTime FormFeaturedFieldLastModified { get; set; }
        [StringLength(200)]
        public string FormFeaturedFieldDisplayName { get; set; } = null!;
        public string FormFeaturedFieldFormDefinition { get; set; } = null!;
        [StringLength(200)]
        public string? FormFeaturedFieldMapping { get; set; }
        public int FormFeaturedFieldOrder { get; set; }
        public string? FormFeaturedFieldDescription { get; set; }
        [StringLength(200)]
        public string FormFeaturedFieldName { get; set; } = null!;
        [StringLength(200)]
        public string FormFeaturedFieldIconClass { get; set; } = null!;
    }
}
