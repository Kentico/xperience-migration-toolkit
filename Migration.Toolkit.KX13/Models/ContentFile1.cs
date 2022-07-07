using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migration.Toolkit.KX13.Models
{
    [Table("CONTENT_File_1")]
    public partial class ContentFile1
    {
        [Key]
        [Column("File_1ID")]
        public int File1id { get; set; }
        [StringLength(100)]
        public string FileName { get; set; } = null!;
        [StringLength(500)]
        public string? FileDescription { get; set; }
        public Guid? FileAttachment { get; set; }
    }
}
