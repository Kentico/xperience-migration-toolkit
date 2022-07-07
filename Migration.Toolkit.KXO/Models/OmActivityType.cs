using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("OM_ActivityType")]
    public partial class OmActivityType
    {
        [Key]
        [Column("ActivityTypeID")]
        public int ActivityTypeId { get; set; }
        [StringLength(250)]
        public string ActivityTypeDisplayName { get; set; } = null!;
        [StringLength(250)]
        public string ActivityTypeName { get; set; } = null!;
        public bool? ActivityTypeEnabled { get; set; }
        public bool? ActivityTypeIsCustom { get; set; }
        public string? ActivityTypeDescription { get; set; }
        public bool? ActivityTypeManualCreationAllowed { get; set; }
        [StringLength(200)]
        public string? ActivityTypeMainFormControl { get; set; }
        [StringLength(200)]
        public string? ActivityTypeDetailFormControl { get; set; }
        [StringLength(7)]
        public string? ActivityTypeColor { get; set; }
        [StringLength(200)]
        public string? ActivityTypeItemObjectType { get; set; }
        [StringLength(200)]
        public string? ActivityTypeItemDetailObjectType { get; set; }
    }
}
