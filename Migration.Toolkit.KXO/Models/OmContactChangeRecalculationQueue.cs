using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("OM_ContactChangeRecalculationQueue")]
    public partial class OmContactChangeRecalculationQueue
    {
        [Key]
        [Column("ContactChangeRecalculationQueueID")]
        public int ContactChangeRecalculationQueueId { get; set; }
        [Column("ContactChangeRecalculationQueueContactID")]
        public int ContactChangeRecalculationQueueContactId { get; set; }
        public string? ContactChangeRecalculationQueueChangedColumns { get; set; }
        public bool ContactChangeRecalculationQueueContactIsNew { get; set; }
        public bool ContactChangeRecalculationQueueContactWasMerged { get; set; }
    }
}
