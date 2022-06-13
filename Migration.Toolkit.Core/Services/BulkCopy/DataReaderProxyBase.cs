using System.Data;
using System.Data.Common;

namespace Migration.Toolkit.Core.Services.BulkCopy;

public class DataReaderProxyBase: IDataReader
{
    protected readonly IDataReader _innerReader;

    public DataReaderProxyBase(IDataReader innerReader)
    {
        _innerReader = innerReader;
    }

    public virtual int FieldCount => _innerReader.FieldCount;
    public virtual int Depth => _innerReader.Depth;
    public virtual bool IsClosed => _innerReader.IsClosed;
    public virtual int RecordsAffected => _innerReader.RecordsAffected;

    public virtual bool GetBoolean(int i) => _innerReader.GetBoolean(i);
    public virtual byte GetByte(int i) => _innerReader.GetByte(i);

    public virtual long GetBytes(int i, long fieldOffset, byte[]? buffer, int bufferoffset, int length) =>
        _innerReader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);

    public virtual char GetChar(int i) => _innerReader.GetChar(i);

    public virtual long GetChars(int i, long fieldoffset, char[]? buffer, int bufferoffset, int length) =>
        _innerReader.GetChars(i, fieldoffset, buffer, bufferoffset, length);

    public virtual IDataReader GetData(int i) => _innerReader.GetData(i);
    public virtual string GetDataTypeName(int i) => _innerReader.GetDataTypeName(i);
    public virtual DateTime GetDateTime(int i) => _innerReader.GetDateTime(i);
    public virtual decimal GetDecimal(int i) => _innerReader.GetDecimal(i);
    public virtual double GetDouble(int i) => _innerReader.GetDouble(i);
    public virtual Type GetFieldType(int i) => _innerReader.GetFieldType(i);
    public virtual float GetFloat(int i) => _innerReader.GetFloat(i);
    public virtual Guid GetGuid(int i) => _innerReader.GetGuid(i);
    public virtual short GetInt16(int i) => _innerReader.GetInt16(i);
    public virtual int GetInt32(int i) => _innerReader.GetInt32(i);
    public virtual long GetInt64(int i) => _innerReader.GetInt64(i);
    public virtual string GetName(int i) => _innerReader.GetName(i);
    public virtual int GetOrdinal(string name) => _innerReader.GetOrdinal(name);
    public virtual string GetString(int i) => _innerReader.GetString(i);
    public virtual object GetValue(int i) => _innerReader.GetValue(i);
    public virtual int GetValues(object[] values) => _innerReader.GetValues(values);
    public virtual bool IsDBNull(int i) => _innerReader.IsDBNull(i);
    public virtual object this[int i] => _innerReader[i];
    public virtual object this[string name] => _innerReader[name];
    public virtual void Dispose() => _innerReader.Dispose();
    public virtual void Close() => _innerReader.Close();
    public virtual DataTable? GetSchemaTable() => _innerReader.GetSchemaTable();
    public virtual bool NextResult() => _innerReader.NextResult();
    public virtual bool Read() => _innerReader.Read();
}