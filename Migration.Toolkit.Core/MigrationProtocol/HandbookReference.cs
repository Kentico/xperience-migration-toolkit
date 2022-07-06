using System.Reflection;
using System.Text;

namespace Migration.Toolkit.Core.MigrationProtocol;

// TODO tk: 2022-05-18 ref uri to wiki 
public class HandbookReference
{
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append($"NeedManualAction: {NeedManualAction}, ReferenceName: {ReferenceName}, AdditionalInfo: {AdditionalInfo}");
        if (this.Data == null) return sb.ToString();

        var arr = this.Data.ToArray();
        for (var i = 0; i < arr.Length; i++)
        {
            var (key, value) = arr[i];
            sb.Append($"{key}: {value}");

            if (i < arr.Length - 1)
            {
                sb.Append(", ");
            }
        }
        return sb.ToString();
    }

    public bool NeedManualAction { get; private set; } = false;
    public string ReferenceName { get; }
    public string? AdditionalInfo { get; }
    public Dictionary<string, object?>? Data { get; private set; } = null;

    public HandbookReference(string referenceName, string? additionalInfo = null)
    {
        this.ReferenceName = referenceName;
        this.AdditionalInfo = additionalInfo;
    }

    public HandbookReference WithData(object data)
    {
        this.Data ??= new();
        var dataUpdate = data.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .ToDictionary(p => p.Name, p => p.GetMethod.Invoke(data, Array.Empty<object>())
            );
        foreach (var (key, value) in dataUpdate)
        {
            this.Data.Add(key, value);   
        }
        
        return this;
    }

    public HandbookReference NeedsManualAction()
    {
        this.NeedManualAction = true;
        return this;
    }
} 