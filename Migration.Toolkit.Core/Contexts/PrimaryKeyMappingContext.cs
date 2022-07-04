using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Migration.Toolkit.Common;
using Migration.Toolkit.Core.Services;

namespace Migration.Toolkit.Core.Contexts;

public class PrimaryKeyMappingContext
{
    private Dictionary<string, int> _mappings = new(StringComparer.OrdinalIgnoreCase); 
    private readonly ILogger<PrimaryKeyMappingContext> _logger;
    private readonly IPrimaryKeyLocatorService _primaryKeyLocatorService;

    public PrimaryKeyMappingContext(ILogger<PrimaryKeyMappingContext> logger, IPrimaryKeyLocatorService primaryKeyLocatorService)
    {
        _logger = logger;
        _primaryKeyLocatorService = primaryKeyLocatorService;
    }

    public void SetMapping(Type type, string keyName, int sourceId, int targetId) 
    {
        Debug.Assert(sourceId > 0, "sourceId > 0");
        Debug.Assert(targetId > 0, "targetId > 0");
        
        var foundProp = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).FirstOrDefault(p => p.Name.Equals(keyName, StringComparison.OrdinalIgnoreCase));
        
        Debug.Assert(foundProp != null, "foundProp != null");
        
        var fullKeyName = $"{type.FullName}.{foundProp.Name}.{sourceId}";
        
        _mappings[fullKeyName] = targetId;
        _logger.LogTrace("Primary key for {fullKeyName} stored. {sourceId} maps to {targetId}", fullKeyName, sourceId, targetId);
    }
    
    public void SetMapping<T>(Expression<Func<T, object>> keyNameSelector, int sourceId, int targetId) 
    {
        Debug.Assert(sourceId > 0, "sourceId > 0");
        Debug.Assert(targetId > 0, "targetId > 0");
        
        var fullKeyName = $"{typeof(T).FullName}.{keyNameSelector.GetMemberName()}.{sourceId}";
        
        _mappings[fullKeyName] = targetId;
        _logger.LogTrace("Primary key for {fullKeyName} stored. {sourceId} maps to {targetId}", fullKeyName, sourceId, targetId);
    }
    
    public int RequireMapFromSource<T>(Expression<Func<T, object>> keyNameSelector, int sourceId)
    {
        var fullKeyName = $"{typeof(T).FullName}.{keyNameSelector.GetMemberName()}.{sourceId}";
        if (sourceId == 0)
        {
            throw new MappingFailureException(fullKeyName, $"Cannot satisfy required mapping {fullKeyName} - source Id cannot be 0.");
        }

        if (_mappings.TryGetValue(fullKeyName, out var resultId))
        {
            _logger.LogTrace("{key} resolved as {value}", fullKeyName, resultId);
            return resultId;
        }
        
        _logger.LogTrace("TryLocate {key}", fullKeyName);
        if(_primaryKeyLocatorService.TryLocate(keyNameSelector, sourceId, out var targetId))
        {
            SetMapping(keyNameSelector, sourceId, targetId); // cache id
            _logger.LogTrace("{key} located as {value}", fullKeyName, resultId);
            return targetId;
        }
        
        throw new MappingFailureException(fullKeyName, "Target entity is missing");
    }
    
    public bool TryRequireMapFromSource<T>(Expression<Func<T, object>> keyNameSelector, int sourceId, out int targetIdResult)
    {
        targetIdResult = -1;
        var fullKeyName = $"{typeof(T).FullName}.{keyNameSelector.GetMemberName()}.{sourceId}";
        if (sourceId == 0)
        {
            throw new MappingFailureException(fullKeyName, $"Cannot satisfy required mapping {fullKeyName} - source Id cannot be 0.");
        }

        if (_mappings.TryGetValue(fullKeyName, out var resultId))
        {
            _logger.LogTrace("{key} resolved as {value}", fullKeyName, resultId);
            targetIdResult = resultId;
            return true;
        }
        
        _logger.LogTrace("TryLocate {key}", fullKeyName);
        if(_primaryKeyLocatorService.TryLocate(keyNameSelector, sourceId, out var targetId))
        {
            SetMapping(keyNameSelector, sourceId, targetId); // cache id
            _logger.LogTrace("{key} located as {value}", fullKeyName, resultId);
            targetIdResult = resultId;
            return true;
        }

        return false;
    }
    
    public int? MapFromSource<T>(Expression<Func<T, object>> keyNameSelector, int? sourceId)
    {
        if (sourceId == null) return null;
        
        var fullKeyName = $"{typeof(T).FullName}.{keyNameSelector.GetMemberName()}.{sourceId}";
        if (sourceId == 0)
        {
            // TODO tk: 2022-05-31 source data autofix applied
            _logger.LogWarning("{key} Key locator invalid argument, cannot supply 0 as argument", fullKeyName);
            return null;
        }

        if (_mappings.TryGetValue(fullKeyName, out var resultId))
        {
            _logger.LogTrace("{key} resolved as {value}", fullKeyName, resultId);
            return resultId;
        }
        if (sourceId is not { } sid)
        {
            return null;
        }
        
        _logger.LogTrace("TryLocate {key}", fullKeyName);
        if(_primaryKeyLocatorService.TryLocate(keyNameSelector, sid, out var targetId))
        {
            SetMapping(keyNameSelector, sid, targetId); // cache id
            _logger.LogTrace("{key} located as {value}", fullKeyName, resultId);
            return targetId;
        }

        throw new MappingFailureException(fullKeyName, $"Target entity is missing");
    }

    public int? MapFromSourceOrNull<T>(Expression<Func<T, object>> keyNameSelector, int? sourceId)
    {
        if (sourceId == null) return null;
        
        var fullKeyName = $"{typeof(T).FullName}.{keyNameSelector.GetMemberName()}.{sourceId}";
        if (sourceId == 0)
        {
            // TODO tk: 2022-05-31 source data autofix applied
            _logger.LogWarning("{key} Key locator invalid argument, cannot supply 0 as argument", fullKeyName);
            return null;
        }
        
        if (_mappings.TryGetValue(fullKeyName, out var resultId))
        {
            _logger.LogTrace("{key} resolved as {value}", fullKeyName, resultId);
            return resultId;
        }
        if (sourceId is not { } sid)
        {
            return null;
        }
        
        _logger.LogTrace("TryLocate {key}", fullKeyName);
        if(_primaryKeyLocatorService.TryLocate(keyNameSelector, sid, out var targetId))
        {
            SetMapping(keyNameSelector, sid, targetId); // cache id
            _logger.LogTrace("{key} located as {value}", fullKeyName, resultId);
            return targetId;
        }

        return null;
    }
}