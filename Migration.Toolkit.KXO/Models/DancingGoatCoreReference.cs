using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("DancingGoatCore_Reference")]
    public partial class DancingGoatCoreReference
    {
        [Key]
        [Column("ReferenceID")]
        public int ReferenceId { get; set; }
        [StringLength(200)]
        public string ReferenceName { get; set; } = null!;
        [StringLength(200)]
        public string ReferenceDescription { get; set; } = null!;
        public string ReferenceText { get; set; } = null!;
        public string ReferenceImage { get; set; } = null!;
    }
}
