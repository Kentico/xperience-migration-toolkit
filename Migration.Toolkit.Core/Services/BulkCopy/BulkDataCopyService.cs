using System.Data;
using System.Data.SqlClient;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Migration.Toolkit.Common;

namespace Migration.Toolkit.Core.Services.BulkCopy;

public class BulkDataCopyService
{
    private readonly ToolkitConfiguration _configuration;
    private readonly ILogger<BulkDataCopyService> _logger;

    public BulkDataCopyService(ToolkitConfiguration configuration, ILogger<BulkDataCopyService> logger)
    {
        _configuration = configuration;
        this._logger = logger;
    }

    // use in case fast object to table insertion is needed
    // // TODO tk: 2022-05-31 remove or use and fully implement
    // public void Copy<TSource>(IEnumerable<TSource> sourceData, string destinationTableName, Func<string, bool> columnNameFilter) where TSource: class
    // {
    //     using var sqlBulkCopy = new SqlBulkCopy(_configuration.TargetConnectionString);
    //     
    //     sqlBulkCopy.DestinationTableName = destinationTableName;
    //     
    //     BulkDataObjectAdapter<TSource>.UpdateColumnMappingsSameColumnNames(sqlBulkCopy.ColumnMappings, columnNameFilter);
    //     using var reader = BulkDataObjectAdapter<TSource>.Adapt(sourceData);
    //     sqlBulkCopy.WriteToServer(reader);
    // }
    //
    // // TODO tk: 2022-05-31 remove is not needed or fully implement 
    // public void Copy<TSource>(IDataReader sourceData, string destinationTableName) where TSource: class
    // {
    //     using var sqlBulkCopy = new SqlBulkCopy(_configuration.TargetConnectionString);
    //     
    //     sqlBulkCopy.DestinationTableName = destinationTableName;
    //     sqlBulkCopy.WriteToServer(sourceData);
    // }

    public bool CheckIfDataExistsInTargetTable(string tableName)
    {
        using var targetConnection = new SqlConnection(_configuration.TargetConnectionString);
        using var command = targetConnection.CreateCommand();
        var query = $"SELECT COUNT(*) FROM {tableName}";
        command.CommandText = query;
        
        
        targetConnection.Open();
        return ((int)command.ExecuteScalar()) > 0;
    }

    public void CopyTableToTable(BulkCopyRequest request)
    {
        var (tableName, columnFilter, dataFilter, batchSize, columns, valueInterceptor) = request;
        
        _logger.LogInformation("Copy of {tableName} started", tableName);

        var sourceColumns = columns == null
            ? GetSqlTableColumns(tableName, _configuration.SourceConnectionString)
                .OrderBy(x => x.ordinalPosition)
                .ToArray()
            : columns.Select((c, idx) => (c, idx)).ToArray();
        
        using var sourceConnection = new SqlConnection(_configuration.SourceConnectionString);
        using var command = sourceConnection.CreateCommand();
        using var sqlBulkCopy = new SqlBulkCopy(_configuration.TargetConnectionString, SqlBulkCopyOptions.KeepIdentity);

        sqlBulkCopy.BatchSize = batchSize;
        // TODO tk: 2022-05-31  sqlBulkCopy.EnableStreaming
        // TODO tk: 2022-05-31  sqlBulkCopy.BulkCopyTimeout
        
        sqlBulkCopy.NotifyAfter = 5000;
        sqlBulkCopy.SqlRowsCopied += (sender, args) =>
        {
            _logger.LogTrace("Copy '{tableName}': Rows copied={rows}", tableName, args.RowsCopied);
        };
        
        
        var selectQuery = BuildSelectQuery(tableName, sourceColumns).ToString();
        
        sqlBulkCopy.DestinationTableName = tableName;
        foreach (var (columnName, ordinalPosition) in sourceColumns)
        {
            if (!columnFilter(columnName))
            {
                continue;
            }
            sqlBulkCopy.ColumnMappings.Add(columnName, columnName);
        }
        command.CommandText = selectQuery;
        command.CommandType = CommandType.Text;
        sourceConnection.Open();
        using var reader = command.ExecuteReader();
        var filteredReader = new FilteredDbDataReader<SqlDataReader>(reader, dataFilter);
        IDataReader readerPipeline = filteredReader;
        if (valueInterceptor != null)
        {
            readerPipeline = new ValueInterceptingReader(readerPipeline, valueInterceptor, sourceColumns);
        }
        sqlBulkCopy.WriteToServer(readerPipeline);
        
        _logger.LogInformation("Copy of {tableName} finished! Total={total}, TotalCopied={totalCopied}", tableName, filteredReader.TotalItems, filteredReader.TotalNonFiltered);
    }

    private static StringBuilder BuildSelectQuery(string tableName, (string columnName, int ordinalPosition)[] sourceColumns)
    {
        StringBuilder selectBuilder = new StringBuilder();
        selectBuilder.Append("SELECT ");
        for (var i = 0; i < sourceColumns.Length; i++)
        {
            var (columnName, ordinalPosition) = sourceColumns[i];
            selectBuilder.Append(columnName);
            if (i < sourceColumns.Length - 1)
            {
                selectBuilder.Append(", ");
            }
        }

        selectBuilder.Append($" FROM {tableName}");
        return selectBuilder;
    }

    public IEnumerable<(string columnName, int ordinalPosition)> GetSqlTableColumns(string tableName, string connectionString)
    {
        using var sourceConnection = new SqlConnection(connectionString);
        using var cmd = sourceConnection.CreateCommand();

        cmd.CommandText = @"SELECT * FROM INFORMATION_SCHEMA.COLUMNS JOIN INFORMATION_SCHEMA.TABLES ON COLUMNS.TABLE_NAME = TABLES.TABLE_NAME
        WHERE TABLES.TABLE_NAME = @tableName";
        
        cmd.Parameters.AddWithValue("tableName", tableName);

        sourceConnection.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            // TODO tk: 2022-05-31 IS_NULLABLE, DATA_TYPE, ... check column compatibility
            yield return (reader.GetString("COLUMN_NAME"), reader.GetInt32("ORDINAL_POSITION"));
        }
    }
}