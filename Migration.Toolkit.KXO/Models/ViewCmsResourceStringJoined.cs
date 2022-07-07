using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Keyless]
    public partial class ViewCmsResourceStringJoined
    {
        [Column("StringID")]
        public int StringId { get; set; }
        [StringLength(200)]
        public string StringKey { get; set; } = null!;
        public bool StringIsCustom { get; set; }
        [Column("TranslationID")]
        public int? TranslationId { get; set; }
        [Column("TranslationStringID")]
        public int? TranslationStringId { get; set; }
        [Column("TranslationCultureID")]
        public int? TranslationCultureId { get; set; }
        public string? TranslationText { get; set; }
        [Column("CultureID")]
        public int? CultureId { get; set; }
        [StringLength(200)]
        public string? CultureName { get; set; }
        [StringLength(50)]
        public string? CultureCode { get; set; }
        [Column("CultureGUID")]
        public Guid? CultureGuid { get; set; }
        public DateTime? CultureLastModified { get; set; }
    }
}
