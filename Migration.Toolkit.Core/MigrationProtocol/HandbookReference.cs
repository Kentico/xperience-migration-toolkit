using System.Reflection;
using System.Text;
using Migration.Toolkit.Core.Helpers;
using Org.BouncyCastle.Tls.Crypto;

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

    public HandbookReference WithId(string idName, object idValue)
    {
        this.WithData(idName, idValue);
        return this;
    }
    
    public HandbookReference WithMessage(string message)
    {
        this.Data ??= new();
        
        var msgNum = 0;
        string msgKey;
        do
        {
            msgKey = $"Message#{msgNum++}";
        } while (this.Data.ContainsKey(msgKey));

        this.WithData(msgKey, message);
        
        return this;
    }

    public HandbookReference WithData(string key, object value)
    {
        this.Data ??= new();
        this.Data.Add(key, value);

        return this;
    }

    public HandbookReference WithData<TValue>(Dictionary<string, TValue> data)
    {
        this.Data ??= new();
        foreach (var (key, value) in data)
        {
            this.Data.Add(key, value);
        }

        return this;
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
    
    public HandbookReference WithIdentityPrint<T>(T model)
    {
        this.Data ??= new();
        this.Data.Add("Entity", EntityPrinter.GetEntityIdentityPrint(model));
        return this;
    }

    public HandbookReference NeedsManualAction()
    {
        this.NeedManualAction = true;
        return this;
    }
}