using Microsoft.Data.SqlClient;
using Migration.Toolkit.Common;

namespace Migration.Toolkit.Core.Services;

public class CoupledDataService
{
    private readonly ToolkitConfiguration _configuration;

    public CoupledDataService(ToolkitConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public Dictionary<string, object>? GetSourceCoupledDataRow(string tableName, string primaryKeyColumn, int? coupledDataId)
    {
        ArgumentNullException.ThrowIfNull(tableName);
        ArgumentNullException.ThrowIfNull(primaryKeyColumn);
        ArgumentNullException.ThrowIfNull(coupledDataId);
        
        using var targetConnection = new SqlConnection(_configuration.SourceConnectionString);
        using var command = targetConnection.CreateCommand();
        var query = $"SELECT * FROM {tableName} WHERE {primaryKeyColumn} = @{primaryKeyColumn}";
        command.CommandText = query;
        command.Parameters.AddWithValue(primaryKeyColumn, coupledDataId);
        
        targetConnection.Open();
        using var reader = command.ExecuteReader();

        var result = new Dictionary<string, object>();
        if (reader.Read())
        {
            for (var i = 0; i < reader.FieldCount; i++)
            {
                result.Add(reader.GetName(i), reader.GetValue(i));
            }

            return result;
        }
        
        return null;
    }
}