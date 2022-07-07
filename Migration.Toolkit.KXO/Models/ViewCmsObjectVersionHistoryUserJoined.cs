using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Keyless]
    public partial class ViewCmsObjectVersionHistoryUserJoined
    {
        [Column("VersionID")]
        public int VersionId { get; set; }
        [Column("VersionObjectID")]
        public int? VersionObjectId { get; set; }
        [StringLength(100)]
        public string VersionObjectType { get; set; } = null!;
        [Column("VersionObjectSiteID")]
        public int? VersionObjectSiteId { get; set; }
        [StringLength(450)]
        public string VersionObjectDisplayName { get; set; } = null!;
        [Column("VersionXML")]
        public string VersionXml { get; set; } = null!;
        [Column("VersionBinaryDataXML")]
        public string? VersionBinaryDataXml { get; set; }
        [Column("VersionModifiedByUserID")]
        public int? VersionModifiedByUserId { get; set; }
        public DateTime VersionModifiedWhen { get; set; }
        [Column("VersionDeletedByUserID")]
        public int? VersionDeletedByUserId { get; set; }
        public DateTime? VersionDeletedWhen { get; set; }
        [StringLength(50)]
        public string VersionNumber { get; set; } = null!;
        [Column("VersionSiteBindingIDs")]
        public string? VersionSiteBindingIds { get; set; }
        public string? VersionComment { get; set; }
        [Column("UserID")]
        public int? UserId { get; set; }
        [StringLength(254)]
        public string? UserName { get; set; }
        [StringLength(100)]
        public string? FirstName { get; set; }
        [StringLength(100)]
        public string? LastName { get; set; }
        [StringLength(254)]
        public string? Email { get; set; }
        [StringLength(100)]
        public string? UserPassword { get; set; }
        public bool? UserEnabled { get; set; }
        public DateTime? UserCreated { get; set; }
        public DateTime? LastLogon { get; set; }
        [Column("UserGUID")]
        public Guid? UserGuid { get; set; }
        public DateTime? UserLastModified { get; set; }
        [StringLength(72)]
        public string? UserSecurityStamp { get; set; }
        public DateTime? UserPasswordLastChanged { get; set; }
        public bool? UserIsPendingRegistration { get; set; }
        public DateTime? UserRegistrationLinkExpiration { get; set; }
        public bool? UserAdministrationAccess { get; set; }
    }
}
