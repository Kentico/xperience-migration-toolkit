using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("OM_ABTest")]
    [Index("AbtestSiteId", Name = "IX_OM_ABTest_SiteID")]
    public partial class OmAbtest
    {
        public OmAbtest()
        {
            OmAbvariantData = new HashSet<OmAbvariantDatum>();
        }

        [Key]
        [Column("ABTestID")]
        public int AbtestId { get; set; }
        [Column("ABTestName")]
        [StringLength(50)]
        public string AbtestName { get; set; } = null!;
        [Column("ABTestDescription")]
        public string? AbtestDescription { get; set; }
        [Column("ABTestCulture")]
        [StringLength(50)]
        public string? AbtestCulture { get; set; }
        [Column("ABTestOriginalPage")]
        [StringLength(450)]
        public string AbtestOriginalPage { get; set; } = null!;
        [Column("ABTestOpenFrom")]
        public DateTime? AbtestOpenFrom { get; set; }
        [Column("ABTestOpenTo")]
        public DateTime? AbtestOpenTo { get; set; }
        [Column("ABTestSiteID")]
        public int AbtestSiteId { get; set; }
        [Column("ABTestGUID")]
        public Guid AbtestGuid { get; set; }
        [Column("ABTestLastModified")]
        public DateTime AbtestLastModified { get; set; }
        [Column("ABTestDisplayName")]
        [StringLength(100)]
        public string AbtestDisplayName { get; set; } = null!;
        [Column("ABTestIncludedTraffic")]
        public int AbtestIncludedTraffic { get; set; }
        [Column("ABTestVisitorTargeting")]
        public string? AbtestVisitorTargeting { get; set; }
        [Column("ABTestConversions")]
        public string? AbtestConversions { get; set; }
        [Column("ABTestWinnerGUID")]
        public Guid? AbtestWinnerGuid { get; set; }

        [ForeignKey("AbtestSiteId")]
        [InverseProperty("OmAbtests")]
        public virtual CmsSite AbtestSite { get; set; } = null!;
        [InverseProperty("AbvariantTest")]
        public virtual ICollection<OmAbvariantDatum> OmAbvariantData { get; set; }
    }
}
