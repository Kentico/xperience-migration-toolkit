using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Keyless]
    public partial class ViewOmAccountContactContactJoined
    {
        [Column("ContactID")]
        public int ContactId { get; set; }
        [StringLength(100)]
        public string? ContactFirstName { get; set; }
        [StringLength(100)]
        public string? ContactMiddleName { get; set; }
        [StringLength(100)]
        public string? ContactLastName { get; set; }
        [StringLength(254)]
        public string? ContactEmail { get; set; }
        [Column("AccountID")]
        public int AccountId { get; set; }
        [Column("AccountContactID")]
        public int AccountContactId { get; set; }
        [Column("ContactCountryID")]
        public int? ContactCountryId { get; set; }
        [Column("ContactStatusID")]
        public int? ContactStatusId { get; set; }
        [Column("ContactRoleID")]
        public int? ContactRoleId { get; set; }
    }
}
