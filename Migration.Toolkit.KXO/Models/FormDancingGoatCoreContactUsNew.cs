using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("Form_DancingGoatCore_ContactUsNew")]
    public partial class FormDancingGoatCoreContactUsNew
    {
        [Key]
        [Column("DancingGoatCoreContactUsNewID")]
        public int DancingGoatCoreContactUsNewId { get; set; }
        public DateTime FormInserted { get; set; }
        public DateTime FormUpdated { get; set; }
        [StringLength(500)]
        public string? UserFirstName { get; set; }
        [StringLength(500)]
        public string? UserLastName { get; set; }
        [StringLength(500)]
        public string UserEmail { get; set; } = null!;
        public string UserMessage { get; set; } = null!;
    }
}
