using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_User")]
    [Index("Email", Name = "IX_CMS_User_Email", IsUnique = true)]
    [Index("UserGuid", Name = "IX_CMS_User_UserGUID", IsUnique = true)]
    [Index("UserName", Name = "IX_CMS_User_UserName", IsUnique = true)]
    public partial class CmsUser
    {
        public CmsUser()
        {
            CmsAutomationHistories = new HashSet<CmsAutomationHistory>();
            CmsAutomationStates = new HashSet<CmsAutomationState>();
            CmsDocumentDocumentCheckedOutByUsers = new HashSet<CmsDocument>();
            CmsDocumentDocumentCreatedByUsers = new HashSet<CmsDocument>();
            CmsDocumentDocumentModifiedByUsers = new HashSet<CmsDocument>();
            CmsExternalLogins = new HashSet<CmsExternalLogin>();
            CmsMacroIdentities = new HashSet<CmsMacroIdentity>();
            CmsMembershipUsers = new HashSet<CmsMembershipUser>();
            CmsObjectSettings = new HashSet<CmsObjectSetting>();
            CmsObjectVersionHistoryVersionDeletedByUsers = new HashSet<CmsObjectVersionHistory>();
            CmsObjectVersionHistoryVersionModifiedByUsers = new HashSet<CmsObjectVersionHistory>();
            CmsPersonalizations = new HashSet<CmsPersonalization>();
            CmsScheduledTasks = new HashSet<CmsScheduledTask>();
            CmsTrees = new HashSet<CmsTree>();
            CmsUserCultures = new HashSet<CmsUserCulture>();
            CmsUserRoles = new HashSet<CmsUserRole>();
            CmsUserSites = new HashSet<CmsUserSite>();
            CmsVersionHistoryModifiedByUsers = new HashSet<CmsVersionHistory>();
            CmsVersionHistoryVersionDeletedByUsers = new HashSet<CmsVersionHistory>();
            CmsWorkflowHistories = new HashSet<CmsWorkflowHistory>();
            CmsWorkflowStepUsers = new HashSet<CmsWorkflowStepUser>();
            ExportHistories = new HashSet<ExportHistory>();
            MediaFileFileCreatedByUsers = new HashSet<MediaFile>();
            MediaFileFileModifiedByUsers = new HashSet<MediaFile>();
            OmAccounts = new HashSet<OmAccount>();
            OmContacts = new HashSet<OmContact>();
            StagingTaskUsers = new HashSet<StagingTaskUser>();
            Workflows = new HashSet<CmsWorkflow>();
        }

        [Key]
        [Column("UserID")]
        public int UserId { get; set; }
        [StringLength(254)]
        public string UserName { get; set; } = null!;
        [StringLength(100)]
        public string? FirstName { get; set; }
        [StringLength(100)]
        public string? LastName { get; set; }
        [StringLength(254)]
        public string? Email { get; set; }
        [StringLength(100)]
        public string UserPassword { get; set; } = null!;
        public bool UserEnabled { get; set; }
        public DateTime? UserCreated { get; set; }
        public DateTime? LastLogon { get; set; }
        [Column("UserGUID")]
        public Guid UserGuid { get; set; }
        public DateTime UserLastModified { get; set; }
        [StringLength(72)]
        public string? UserSecurityStamp { get; set; }
        public DateTime? UserPasswordLastChanged { get; set; }
        public bool UserIsPendingRegistration { get; set; }
        public DateTime? UserRegistrationLinkExpiration { get; set; }
        public bool UserAdministrationAccess { get; set; }

        [InverseProperty("UserMacroIdentityUser")]
        public virtual CmsUserMacroIdentity CmsUserMacroIdentity { get; set; } = null!;
        [InverseProperty("User")]
        public virtual StagingTaskGroupUser StagingTaskGroupUser { get; set; } = null!;
        [InverseProperty("HistoryApprovedByUser")]
        public virtual ICollection<CmsAutomationHistory> CmsAutomationHistories { get; set; }
        [InverseProperty("StateUser")]
        public virtual ICollection<CmsAutomationState> CmsAutomationStates { get; set; }
        [InverseProperty("DocumentCheckedOutByUser")]
        public virtual ICollection<CmsDocument> CmsDocumentDocumentCheckedOutByUsers { get; set; }
        [InverseProperty("DocumentCreatedByUser")]
        public virtual ICollection<CmsDocument> CmsDocumentDocumentCreatedByUsers { get; set; }
        [InverseProperty("DocumentModifiedByUser")]
        public virtual ICollection<CmsDocument> CmsDocumentDocumentModifiedByUsers { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<CmsExternalLogin> CmsExternalLogins { get; set; }
        [InverseProperty("MacroIdentityEffectiveUser")]
        public virtual ICollection<CmsMacroIdentity> CmsMacroIdentities { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<CmsMembershipUser> CmsMembershipUsers { get; set; }
        [InverseProperty("ObjectCheckedOutByUser")]
        public virtual ICollection<CmsObjectSetting> CmsObjectSettings { get; set; }
        [InverseProperty("VersionDeletedByUser")]
        public virtual ICollection<CmsObjectVersionHistory> CmsObjectVersionHistoryVersionDeletedByUsers { get; set; }
        [InverseProperty("VersionModifiedByUser")]
        public virtual ICollection<CmsObjectVersionHistory> CmsObjectVersionHistoryVersionModifiedByUsers { get; set; }
        [InverseProperty("PersonalizationUser")]
        public virtual ICollection<CmsPersonalization> CmsPersonalizations { get; set; }
        [InverseProperty("TaskUser")]
        public virtual ICollection<CmsScheduledTask> CmsScheduledTasks { get; set; }
        [InverseProperty("NodeOwnerNavigation")]
        public virtual ICollection<CmsTree> CmsTrees { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<CmsUserCulture> CmsUserCultures { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<CmsUserRole> CmsUserRoles { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<CmsUserSite> CmsUserSites { get; set; }
        [InverseProperty("ModifiedByUser")]
        public virtual ICollection<CmsVersionHistory> CmsVersionHistoryModifiedByUsers { get; set; }
        [InverseProperty("VersionDeletedByUser")]
        public virtual ICollection<CmsVersionHistory> CmsVersionHistoryVersionDeletedByUsers { get; set; }
        [InverseProperty("ApprovedByUser")]
        public virtual ICollection<CmsWorkflowHistory> CmsWorkflowHistories { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<CmsWorkflowStepUser> CmsWorkflowStepUsers { get; set; }
        [InverseProperty("ExportUser")]
        public virtual ICollection<ExportHistory> ExportHistories { get; set; }
        [InverseProperty("FileCreatedByUser")]
        public virtual ICollection<MediaFile> MediaFileFileCreatedByUsers { get; set; }
        [InverseProperty("FileModifiedByUser")]
        public virtual ICollection<MediaFile> MediaFileFileModifiedByUsers { get; set; }
        [InverseProperty("AccountOwnerUser")]
        public virtual ICollection<OmAccount> OmAccounts { get; set; }
        [InverseProperty("ContactOwnerUser")]
        public virtual ICollection<OmContact> OmContacts { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<StagingTaskUser> StagingTaskUsers { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Users")]
        public virtual ICollection<CmsWorkflow> Workflows { get; set; }
    }
}
