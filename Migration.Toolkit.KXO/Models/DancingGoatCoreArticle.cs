using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migration.Toolkit.KXO.Models
{
    [Table("DancingGoatCore_Article")]
    public partial class DancingGoatCoreArticle
    {
        [Key]
        [Column("ArticleID")]
        public int ArticleId { get; set; }
        [StringLength(450)]
        public string ArticleTitle { get; set; } = null!;
        public string? ArticleTeaser { get; set; }
        [StringLength(190)]
        public string ArticleSummary { get; set; } = null!;
        public string ArticleText { get; set; } = null!;
    }
}
