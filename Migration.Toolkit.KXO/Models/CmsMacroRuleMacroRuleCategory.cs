using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_MacroRuleMacroRuleCategory")]
    [Index("MacroRuleCategoryId", Name = "IX_CMS_MacroRuleMacroRuleCategory_MacroRuleCategoryID")]
    [Index("MacroRuleId", Name = "IX_CMS_MacroRuleMacroRuleCategory_MacroRuleID")]
    public partial class CmsMacroRuleMacroRuleCategory
    {
        [Key]
        [Column("MacroRuleMacroRuleCategoryID")]
        public int MacroRuleMacroRuleCategoryId { get; set; }
        [Column("MacroRuleID")]
        public int MacroRuleId { get; set; }
        [Column("MacroRuleCategoryID")]
        public int MacroRuleCategoryId { get; set; }

        [ForeignKey("MacroRuleId")]
        [InverseProperty("CmsMacroRuleMacroRuleCategories")]
        public virtual CmsMacroRule MacroRule { get; set; } = null!;
        [ForeignKey("MacroRuleCategoryId")]
        [InverseProperty("CmsMacroRuleMacroRuleCategories")]
        public virtual CmsMacroRuleCategory MacroRuleCategory { get; set; } = null!;
    }
}
