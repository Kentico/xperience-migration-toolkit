using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("DancingGoatCore_Coffee")]
    public partial class DancingGoatCoreCoffee
    {
        [Key]
        [Column("CoffeeID")]
        public int CoffeeId { get; set; }
        [StringLength(450)]
        public string CoffeeShortDescription { get; set; } = null!;
        public string CoffeeDescription { get; set; } = null!;
        public string? CoffeeImage { get; set; }
    }
}
