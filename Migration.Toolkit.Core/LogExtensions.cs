using Microsoft.Extensions.Logging;
using Migration.Toolkit.Common.Helpers;
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
                logger.LogTrace("Success - {model}", EntityPrinter.PrintKxoModelInfo(result.Item));
                break;
            }
            default:
            {
                throw new ArgumentOutOfRangeException(nameof(mappingResult));
            }
        }

        return mappingResult;
    }

    public static ILogger<T> LogEntitySetAction<T, TEntity>(this ILogger<T> logger, bool newInstance, TEntity entity)
    {
        var entityIdentityPrint = EntityPrinter.GetEntityIdentityPrint(entity);
        logger.LogInformation("Entity {EntityType} was {Action}, {EntityIdentityPrint}", ReflectionHelper<T>.CurrentType.Name, newInstance ? "inserted" : "updated", entityIdentityPrint);
        return logger;
    }
    
    public static ILogger<T> LogEntitySetError<T, TEntity>(this ILogger<T> logger, Exception exception, bool newInstance, TEntity entity)
    {
        var entityIdentityPrint = EntityPrinter.GetEntityIdentityPrint(entity);
        logger.LogError(exception, "Entity {EntityType} failed during {Action}, {EntityIdentityPrint}", ReflectionHelper<T>.CurrentType.Name, newInstance ? "insert" : "update", entityIdentityPrint);
        return logger;
    }
}