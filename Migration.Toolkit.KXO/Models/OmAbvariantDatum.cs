using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("OM_ABVariantData")]
    [Index("AbvariantTestId", "AbvariantGuid", Name = "IX_OM_ABVariantData_ABVariantTestID_ABVariantGUID")]
    public partial class OmAbvariantDatum
    {
        [Key]
        [Column("ABVariantID")]
        public int AbvariantId { get; set; }
        [Column("ABVariantDisplayName")]
        [StringLength(100)]
        public string AbvariantDisplayName { get; set; } = null!;
        [Column("ABVariantGUID")]
        public Guid AbvariantGuid { get; set; }
        [Column("ABVariantTestID")]
        public int AbvariantTestId { get; set; }
        [Column("ABVariantIsOriginal")]
        public bool AbvariantIsOriginal { get; set; }

        [ForeignKey("AbvariantTestId")]
        [InverseProperty("OmAbvariantData")]
        public virtual OmAbtest AbvariantTest { get; set; } = null!;
    }
}
