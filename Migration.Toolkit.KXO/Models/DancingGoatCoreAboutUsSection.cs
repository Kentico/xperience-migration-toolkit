using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("DancingGoatCore_AboutUsSection")]
    public partial class DancingGoatCoreAboutUsSection
    {
        [Key]
        [Column("AboutUsSectionID")]
        public int AboutUsSectionId { get; set; }
        [StringLength(200)]
        public string AboutUsSectionHeading { get; set; } = null!;
        public string? AboutUsSectionImage { get; set; }
        public string AboutUsSectionText { get; set; } = null!;
    }
}
