using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("Form_DancingGoatCore_CoffeeSampleList")]
    public partial class FormDancingGoatCoreCoffeeSampleList
    {
        [Key]
        [Column("DancingGoatCoreCoffeeSampleListID")]
        public int DancingGoatCoreCoffeeSampleListId { get; set; }
        public DateTime FormInserted { get; set; }
        public DateTime FormUpdated { get; set; }
        [StringLength(500)]
        public string FirstName { get; set; } = null!;
        [StringLength(500)]
        public string LastName { get; set; } = null!;
        [StringLength(500)]
        public string Email { get; set; } = null!;
        [StringLength(500)]
        public string Address { get; set; } = null!;
        [StringLength(500)]
        public string City { get; set; } = null!;
        [Column("ZIPCode")]
        [StringLength(500)]
        public string Zipcode { get; set; } = null!;
        [StringLength(500)]
        public string? State { get; set; }
        [StringLength(500)]
        public string Country { get; set; } = null!;
    }
}
