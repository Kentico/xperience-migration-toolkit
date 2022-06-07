using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_AlternativeForm")]
    [Index("FormClassId", "FormName", Name = "IX_CMS_AlternativeForm_FormClassID_FormName")]
    [Index("FormCoupledClassId", Name = "IX_CMS_AlternativeForm_FormCoupledClassID")]
    public partial class CmsAlternativeForm
    {
        [Key]
        [Column("FormID")]
        public int FormId { get; set; }
        [StringLength(100)]
        public string FormDisplayName { get; set; } = null!;
        [StringLength(50)]
        public string FormName { get; set; } = null!;
        [Column("FormClassID")]
        public int FormClassId { get; set; }
        public string? FormDefinition { get; set; }
        [Column("FormGUID")]
        public Guid FormGuid { get; set; }
        public DateTime FormLastModified { get; set; }
        [Column("FormCoupledClassID")]
        public int? FormCoupledClassId { get; set; }
        public bool? FormHideNewParentFields { get; set; }
        [StringLength(400)]
        public string? FormCustomizedColumns { get; set; }
        public bool? FormIsCustom { get; set; }

        [ForeignKey("FormClassId")]
        [InverseProperty("CmsAlternativeFormFormClasses")]
        public virtual CmsClass FormClass { get; set; } = null!;
        [ForeignKey("FormCoupledClassId")]
        [InverseProperty("CmsAlternativeFormFormCoupledClasses")]
        public virtual CmsClass? FormCoupledClass { get; set; }
    }
}
