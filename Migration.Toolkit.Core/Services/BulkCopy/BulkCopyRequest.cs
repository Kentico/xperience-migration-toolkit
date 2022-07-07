using System.Data;
using System.Data.Common;

namespace Migration.Toolkit.Core.Services.BulkCopy;

public record BulkCopyRequest(string TableName, Func<string,bool> ColumnFilter, Func<IDataReader,bool> DataFilter, int BatchSize, List<string>? Columns = null, ValueInterceptor? ValueInterceptor = null, ValueInterceptingReaderSkippedRow? SkippedRowCallback = null, string OrderBy = null);