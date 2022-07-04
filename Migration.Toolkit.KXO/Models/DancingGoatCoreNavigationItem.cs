using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migration.Toolkit.KXO.Models
{
    [Table("DancingGoatCore_NavigationItem")]
    public partial class DancingGoatCoreNavigationItem
    {
        [Key]
        [Column("NavigationItemID")]
        public int NavigationItemId { get; set; }
        public string LinkTo { get; set; } = null!;
    }
}
