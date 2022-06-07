using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migration.Toolkit.KXO.Models
{
    [Table("DancingGoatCore_Contact")]
    public partial class DancingGoatCoreContact
    {
        [Key]
        [Column("ContactID")]
        public int ContactId { get; set; }
        [StringLength(50)]
        public string ContactName { get; set; } = null!;
        [StringLength(50)]
        public string ContactStreet { get; set; } = null!;
        [StringLength(50)]
        public string ContactCity { get; set; } = null!;
        [StringLength(100)]
        public string ContactCountry { get; set; } = null!;
        [StringLength(15)]
        public string ContactZipCode { get; set; } = null!;
        [StringLength(30)]
        public string ContactPhone { get; set; } = null!;
        [StringLength(400)]
        public string ContactEmail { get; set; } = null!;
    }
}
