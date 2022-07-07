using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("OM_VisitorToContact")]
    [Index("VisitorToContactContactId", Name = "IX_OM_VisitorToContact_VisitorToContactContactID")]
    [Index("VisitorToContactVisitorGuid", Name = "IX_OM_VisitorToContact_VisitorToContactVisitorGUID", IsUnique = true)]
    public partial class OmVisitorToContact
    {
        [Key]
        [Column("VisitorToContactID")]
        public int VisitorToContactId { get; set; }
        [Column("VisitorToContactVisitorGUID")]
        public Guid VisitorToContactVisitorGuid { get; set; }
        [Column("VisitorToContactContactID")]
        public int VisitorToContactContactId { get; set; }

        [ForeignKey("VisitorToContactContactId")]
        [InverseProperty("OmVisitorToContacts")]
        public virtual OmContact VisitorToContactContact { get; set; } = null!;
    }
}
