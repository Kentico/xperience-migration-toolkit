using System.Data;
using System.Data.Common;

namespace Migration.Toolkit.Core.Services.BulkCopy;

public class ValueInterceptingReader : DataReaderProxyBase
{
    private readonly Func<int, string, object, object> _valueInterceptor;
    private readonly Dictionary<int, string> _columnOrdinals;

    public ValueInterceptingReader(IDataReader innerReader, Func<int, string, object, object> valueInterceptor,
        SqlColumn[] columnOrdinals) : base(innerReader)
    {
        _valueInterceptor = valueInterceptor;
        _columnOrdinals = columnOrdinals.ToDictionary(x => x.OrdinalPosition, x => x.ColumnName);
    }

    public override object GetValue(int i) => _valueInterceptor.Invoke(i, _columnOrdinals[i], base.GetValue(i));
}
