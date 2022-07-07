using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_SearchTaskAzure")]
    [Index("SearchTaskAzurePriority", Name = "IX_CMS_SearchTaskAzure_SearchTaskAzurePriority")]
    public partial class CmsSearchTaskAzure
    {
        [Key]
        [Column("SearchTaskAzureID")]
        public int SearchTaskAzureId { get; set; }
        [StringLength(100)]
        public string SearchTaskAzureType { get; set; } = null!;
        [StringLength(100)]
        public string? SearchTaskAzureObjectType { get; set; }
        [StringLength(200)]
        public string? SearchTaskAzureMetadata { get; set; }
        [StringLength(600)]
        public string SearchTaskAzureAdditionalData { get; set; } = null!;
        [Column("SearchTaskAzureInitiatorObjectID")]
        public int? SearchTaskAzureInitiatorObjectId { get; set; }
        public int SearchTaskAzurePriority { get; set; }
        public string? SearchTaskAzureErrorMessage { get; set; }
        public DateTime SearchTaskAzureCreated { get; set; }
        [StringLength(100)]
        public string? SearchTaskAzureIndexerName { get; set; }
    }
}
