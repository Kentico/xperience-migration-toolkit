using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Helpers;
using Migration.Toolkit.Core.MigrationProtocol;

namespace Migration.Toolkit.Core;

public static class LogExtensions
{
    public static IModelMappingResult<TResult> Log<TResult>(this IModelMappingResult<TResult> mappingResult, ILogger logger, IMigrationProtocol protocol)
    {
        switch (mappingResult)
        {
            case { Success: false } result:
            {
                if (result is AggregatedResult<TResult> aggregatedResult)
                {
                    foreach (var r in aggregatedResult.Results)
                    {
                        protocol.Append(r.HandbookReference);
                        logger.LogError(r.HandbookReference?.ToString());
                    }
                }
                else
                {
                    protocol.Append(result.HandbookReference);   
                    logger.LogError(result.HandbookReference?.ToString());
                }
                
                break;
            }
            case { Success: true } result:
            {
                logger.LogTrace("Success - {model}", LogHelper.PrintKxoModelInfo(result.Item));
                break;
            }
            default:
            {
                throw new ArgumentOutOfRangeException(nameof(mappingResult));
            }
        }

        return mappingResult;
    }
}