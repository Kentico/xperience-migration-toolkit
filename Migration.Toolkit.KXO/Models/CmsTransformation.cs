using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_Transformation")]
    [Index("TransformationClassId", Name = "IX_CMS_Transformation_TransformationClassID")]
    public partial class CmsTransformation
    {
        [Key]
        [Column("TransformationID")]
        public int TransformationId { get; set; }
        [StringLength(100)]
        public string TransformationName { get; set; } = null!;
        public string TransformationCode { get; set; } = null!;
        [StringLength(50)]
        public string TransformationType { get; set; } = null!;
        [Column("TransformationClassID")]
        public int TransformationClassId { get; set; }
        [Column("TransformationGUID")]
        public Guid TransformationGuid { get; set; }
        public DateTime TransformationLastModified { get; set; }

        [ForeignKey("TransformationClassId")]
        [InverseProperty("CmsTransformations")]
        public virtual CmsClass TransformationClass { get; set; } = null!;
    }
}
