using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CD_Migration")]
    [Index("MigrationName", Name = "IX_CD_Migration_MigrationName", IsUnique = true)]
    public partial class CdMigration
    {
        [Key]
        [Column("MigrationID")]
        public int MigrationId { get; set; }
        [StringLength(255)]
        public string MigrationName { get; set; } = null!;
        [Precision(3)]
        public DateTime DateApplied { get; set; }
        public int? RowsAffected { get; set; }
    }
}
