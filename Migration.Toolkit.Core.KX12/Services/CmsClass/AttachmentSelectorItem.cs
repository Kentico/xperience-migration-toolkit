namespace Migration.Toolkit.Core.KX12.Services.CmsClass;

using Newtonsoft.Json;

/// <summary>Represents an item for the attachment selector.</summary>
public class AttachmentSelectorItem
{
    /// <summary>Attachment GUID.</summary>
    [JsonProperty("fileGuid")]
    public Guid FileGuid { get; set; }
}