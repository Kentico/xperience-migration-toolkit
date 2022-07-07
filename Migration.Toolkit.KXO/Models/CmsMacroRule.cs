using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_MacroRule")]
    public partial class CmsMacroRule
    {
        public CmsMacroRule()
        {
            CmsMacroRuleMacroRuleCategories = new HashSet<CmsMacroRuleMacroRuleCategory>();
        }

        [Key]
        [Column("MacroRuleID")]
        public int MacroRuleId { get; set; }
        [StringLength(200)]
        public string MacroRuleName { get; set; } = null!;
        [StringLength(1000)]
        public string MacroRuleText { get; set; } = null!;
        public string? MacroRuleParameters { get; set; }
        [StringLength(100)]
        public string? MacroRuleResourceName { get; set; }
        public DateTime MacroRuleLastModified { get; set; }
        [Column("MacroRuleGUID")]
        public Guid MacroRuleGuid { get; set; }
        public string MacroRuleCondition { get; set; } = null!;
        [StringLength(500)]
        public string MacroRuleDisplayName { get; set; } = null!;
        public bool? MacroRuleIsCustom { get; set; }
        public bool MacroRuleRequiresContext { get; set; }
        [StringLength(450)]
        public string? MacroRuleDescription { get; set; }
        [StringLength(2500)]
        public string? MacroRuleRequiredData { get; set; }
        public bool? MacroRuleEnabled { get; set; }
        public int? MacroRuleAvailability { get; set; }

        [InverseProperty("MacroRule")]
        public virtual ICollection<CmsMacroRuleMacroRuleCategory> CmsMacroRuleMacroRuleCategories { get; set; }
    }
}
