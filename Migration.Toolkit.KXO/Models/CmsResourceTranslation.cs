using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_ResourceTranslation")]
    [Index("TranslationCultureId", Name = "IX_CMS_ResourceTranslation_TranslationCultureID")]
    [Index("TranslationStringId", Name = "IX_CMS_ResourceTranslation_TranslationStringID")]
    public partial class CmsResourceTranslation
    {
        [Key]
        [Column("TranslationID")]
        public int TranslationId { get; set; }
        [Column("TranslationStringID")]
        public int TranslationStringId { get; set; }
        public string? TranslationText { get; set; }
        [Column("TranslationCultureID")]
        public int TranslationCultureId { get; set; }

        [ForeignKey("TranslationCultureId")]
        [InverseProperty("CmsResourceTranslations")]
        public virtual CmsCulture TranslationCulture { get; set; } = null!;
        [ForeignKey("TranslationStringId")]
        [InverseProperty("CmsResourceTranslations")]
        public virtual CmsResourceString TranslationString { get; set; } = null!;
    }
}
