using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migration.Toolkit.KXO.Models
{
    [Table("DancingGoatCore_Cafe")]
    public partial class DancingGoatCoreCafe
    {
        [Key]
        [Column("CafeID")]
        public int CafeId { get; set; }
        [StringLength(50)]
        public string CafeName { get; set; } = null!;
        public bool? CafeIsCompanyCafe { get; set; }
        [StringLength(50)]
        public string CafeStreet { get; set; } = null!;
        [StringLength(50)]
        public string CafeCity { get; set; } = null!;
        [StringLength(100)]
        public string CafeCountry { get; set; } = null!;
        [StringLength(15)]
        public string CafeZipCode { get; set; } = null!;
        [StringLength(30)]
        public string CafePhone { get; set; } = null!;
        public string? CafePhoto { get; set; }
        [StringLength(200)]
        public string? CafeAdditionalNotes { get; set; }
    }
}
