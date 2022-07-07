using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_UserMacroIdentity")]
    [Index("UserMacroIdentityMacroIdentityId", Name = "IX_CMS_UserMacroIdentity_UserMacroIdentityMacroIdentityID")]
    [Index("UserMacroIdentityUserId", Name = "UQ_CMS_UserMacroIdentity_UserMacroIdentityUserID", IsUnique = true)]
    public partial class CmsUserMacroIdentity
    {
        [Key]
        [Column("UserMacroIdentityID")]
        public int UserMacroIdentityId { get; set; }
        public DateTime UserMacroIdentityLastModified { get; set; }
        [Column("UserMacroIdentityUserID")]
        public int UserMacroIdentityUserId { get; set; }
        [Column("UserMacroIdentityMacroIdentityID")]
        public int? UserMacroIdentityMacroIdentityId { get; set; }
        public Guid UserMacroIdentityUserGuid { get; set; }

        [ForeignKey("UserMacroIdentityMacroIdentityId")]
        [InverseProperty("CmsUserMacroIdentities")]
        public virtual CmsMacroIdentity? UserMacroIdentityMacroIdentity { get; set; }
        [ForeignKey("UserMacroIdentityUserId")]
        [InverseProperty("CmsUserMacroIdentity")]
        public virtual CmsUser UserMacroIdentityUser { get; set; } = null!;
    }
}
