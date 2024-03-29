﻿namespace Migration.Toolkit.K11.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Temp_PageBuilderWidgets")]
public partial class TempPageBuilderWidget
{
    [Key]
    [Column("PageBuilderWidgetsID")]
    public int PageBuilderWidgetsId { get; set; }

    public string? PageBuilderWidgetsConfiguration { get; set; }

    public Guid PageBuilderWidgetsGuid { get; set; }

    public DateTime PageBuilderWidgetsLastModified { get; set; }

    public string? PageBuilderTemplateConfiguration { get; set; }
}
