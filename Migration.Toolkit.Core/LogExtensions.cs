using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Newtonsoft.Json;

namespace Migration.Toolkit.Core;

public static class LogExtensions
{
    public static IModelMappingResult<TResult> Log<T, TResult>(this IModelMappingResult<TResult> mappingResult, ILogger<T> logger)
    {
        switch (mappingResult)
        {
            case { Success: false } result:
            {
                logger.LogError(result.Message);
                break;
            }
            case { Success: true } result:
            {
                logger.LogTrace("Model mapped successfully"); // TODO tk: 2022-06-09 item print
                break;
            }
            // case ModelMappingFailed<TResult>(var message):
            // {
            //     logger.LogError(message);
            //     break;
            // }
            // case ModelMappingFailedKeyMismatch<TResult>(var tResult, var success, var message, var newInstance):
            // {
            //     logger.LogError(message);
            //     break;
            // }
            // case ModelMappingFailedSourceNotDefined<TResult>(var tResult, var success, var message, var newInstance):
            // {
            //     logger.LogError(message);
            //     break;
            // }
            // case ModelMappingSuccess<TResult>(var tResult, var newInstance):
            // {
            //     logger.LogTrace($"Model mapped successfully");
            //     break;
            // }
            default:
            {
                throw new ArgumentOutOfRangeException(nameof(mappingResult));
            }
        }

        return mappingResult;
    }
}