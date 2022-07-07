using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace Migration.Toolkit.Core.Services.BulkCopy;

public record ValueInterceptorResult(object? Value = null, bool OverwriteValue = false, bool SkipDataRow = false)
{
    public static ValueInterceptorResult DoNothing => new ValueInterceptorResult();
    public static ValueInterceptorResult SkipRow => new ValueInterceptorResult(null, false, true);
    public static ValueInterceptorResult ReplaceValue(object? value) => new ValueInterceptorResult(value, true, false);
};
public delegate ValueInterceptorResult ValueInterceptor(int columnOrdinal, string columnName, object value, Dictionary<string, object?> currentRow);
public delegate void ValueInterceptingReaderSkippedRow(Dictionary<string, object?> current);

public class ValueInterceptingReader : DataReaderProxyBase
{
    private readonly ValueInterceptor _valueInterceptor;
    private readonly ValueInterceptingReaderSkippedRow? _skippedValueCallback;
    private readonly Dictionary<int, string> _columnOrdinals;

    private Dictionary<int, object?> _overwrittenValues = new Dictionary<int, object?>();

    public ValueInterceptingReader(IDataReader innerReader, ValueInterceptor valueInterceptor, SqlColumn[] columnOrdinals, ValueInterceptingReaderSkippedRow? skippedValueCallback) : base(innerReader)
    {
        _valueInterceptor = valueInterceptor;
        _skippedValueCallback = skippedValueCallback;
        _columnOrdinals = columnOrdinals.ToDictionary(x => x.OrdinalPosition, x => x.ColumnName);
    }

    public override object? GetValue(int i) => _overwrittenValues.ContainsKey(i) ? _overwrittenValues[i] : base.GetValue(i);

    public override bool Read()
    {
        while (base.Read())
        {
            _overwrittenValues = new Dictionary<int, object?>();
            
            var skipCurrentDataRow = false;
            var currentRow = _columnOrdinals.ToDictionary(k => k.Value, v => base.GetValue(v.Key));
            foreach (var (columnOrdinal, columnName) in this._columnOrdinals)
            {
                
                var (newValue, overwriteValue, skipDataRow) = _valueInterceptor.Invoke(columnOrdinal, columnName, base.GetValue(columnOrdinal), currentRow);
                if (skipDataRow)
                {
                    skipCurrentDataRow = true;
                    break;
                }

                if (overwriteValue)
                {
                    _overwrittenValues[columnOrdinal] = newValue;
                }
            }

            if (skipCurrentDataRow)
            {
                _skippedValueCallback?.Invoke(currentRow);
                continue;
            }
            
            return true;
        }

        return false;
    }
}
