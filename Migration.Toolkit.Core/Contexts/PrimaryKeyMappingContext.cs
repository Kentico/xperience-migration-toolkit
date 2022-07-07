using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Migration.Toolkit.Common;
using Migration.Toolkit.Core.Services;

namespace Migration.Toolkit.Core.Contexts;

public record MapSourceIdResult(bool Success, int? MappedId);

public class PrimaryKeyMappingContext
{
    private Dictionary<string, int> _mappings = new(StringComparer.OrdinalIgnoreCase); 
    private readonly ILogger<PrimaryKeyMappingContext> _logger;
    private readonly IPrimaryKeyLocatorService _primaryKeyLocatorService;
    private readonly ToolkitConfiguration _toolkitConfiguration;

    public PrimaryKeyMappingContext(ILogger<PrimaryKeyMappingContext> logger, IPrimaryKeyLocatorService primaryKeyLocatorService, ToolkitConfiguration toolkitConfiguration)
    {
        _logger = logger;
        _primaryKeyLocatorService = primaryKeyLocatorService;
        _toolkitConfiguration = toolkitConfiguration;
    }

    private int? GetExplicitMapping<T>(string memberName, int? sourceId)
    {
        if (sourceId == null) return null;
        
        var mappings = _toolkitConfiguration.EntityConfigurations.GetEntityConfiguration<T>().ExplicitPrimaryKeyMapping;
        if (mappings.TryGetValue(memberName, out var memberMappings))
        {
            if (memberMappings.TryGetValue($"{sourceId}", out var mappedId))
            {
                return mappedId;
            }
        }

        return null;
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
        // Debug.Assert(sourceId > 0, "sourceId > 0");
        // Debug.Assert(targetId > 0, "targetId > 0");
        
        var fullKeyName = $"{typeof(T).FullName}.{keyNameSelector.GetMemberName()}.{sourceId}";
        
        _mappings[fullKeyName] = targetId;
        // _logger.LogTrace("Primary key for {fullKeyName} stored. {sourceId} maps to {targetId}", fullKeyName, sourceId, targetId);
    }
    
    public int RequireMapFromSource<T>(Expression<Func<T, object>> keyNameSelector, int sourceId)
    {
        var memberName = keyNameSelector.GetMemberName();
        var fullKeyName = $"{typeof(T).FullName}.{memberName}.{sourceId}";
        if (sourceId == 0)
        {
            throw new MappingFailureException(fullKeyName, $"Cannot satisfy required mapping {fullKeyName} - source Id cannot be 0.");
        }

        if (GetExplicitMapping<T>(memberName, sourceId) is { } explicitlyMappedId)
        {
            _logger.LogTrace("{key} resolved as {value} from explicit mapping", fullKeyName, explicitlyMappedId);
            return explicitlyMappedId;
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

    public bool TryRequireMapFromSource<T>(Expression<Func<T, object>> keyNameSelector, int? sourceId, out int targetIdResult)
    {
        targetIdResult = -1;
        if (!(sourceId is int sid))
        {
            return false;
        }

        var memberName = keyNameSelector.GetMemberName();
        var fullKeyName = $"{typeof(T).FullName}.{memberName}.{sid}";
        if (sid == 0)
        {
            throw new MappingFailureException(fullKeyName, $"Cannot satisfy required mapping {fullKeyName} - source Id cannot be 0.");
        }
        
        if (GetExplicitMapping<T>(memberName, sourceId) is { } explicitlyMappedId)
        {
            _logger.LogTrace("{key} resolved as {value} from explicit mapping", fullKeyName, explicitlyMappedId);
            targetIdResult = explicitlyMappedId;
            return true;
        }

        if (_mappings.TryGetValue(fullKeyName, out var resultId))
        {
            _logger.LogTrace("{key} resolved as {value}", fullKeyName, resultId);
            targetIdResult = resultId;
            return true;
        }
        
        _logger.LogTrace("TryLocate {key}", fullKeyName);
        if(_primaryKeyLocatorService.TryLocate(keyNameSelector, sid, out var targetId))
        {
            SetMapping(keyNameSelector, sid, targetId); // cache id
            _logger.LogTrace("{key} located as {value}", fullKeyName, targetId);
            targetIdResult = targetId;
            return true;
        }

        return false;
    }

    public int? MapFromSource<T>(Expression<Func<T, object>> keyNameSelector, int? sourceId)
    {
        if (sourceId == null) return null;

        var memberName = keyNameSelector.GetMemberName();
        var fullKeyName = $"{typeof(T).FullName}.{memberName}.{sourceId}";
        if (sourceId == 0)
        {
            // TODO tk: 2022-05-31 source data autofix applied
            _logger.LogWarning("{key} Key locator invalid argument, cannot supply 0 as argument", fullKeyName);
            return null;
        }
        
        if (GetExplicitMapping<T>(memberName, sourceId) is { } explicitlyMappedId)
        {
            _logger.LogTrace("{key} resolved as {value} from explicit mapping", fullKeyName, explicitlyMappedId);
            return explicitlyMappedId;
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
            _logger.LogTrace("{key} located as {value}", fullKeyName, targetId);
            return targetId;
        }

        throw new MappingFailureException(fullKeyName, $"Target entity is missing");
    }

    public int? MapFromSourceOrNull<T>(Expression<Func<T, object>> keyNameSelector, int? sourceId)
    {
        if (sourceId == null) return null;

        var memberName = keyNameSelector.GetMemberName();
        var fullKeyName = $"{typeof(T).FullName}.{memberName}.{sourceId}";
        if (sourceId == 0)
        {
            // TODO tk: 2022-05-31 source data autofix applied
            _logger.LogWarning("{key} Key locator invalid argument, cannot supply 0 as argument", fullKeyName);
            return null;
        }
        
        if (GetExplicitMapping<T>(memberName, sourceId) is { } explicitlyMappedId)
        {
            _logger.LogTrace("{key} resolved as {value} from explicit mapping", fullKeyName, explicitlyMappedId);
            return explicitlyMappedId;
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
            _logger.LogTrace("{key} located as {value}", fullKeyName, targetId);
            return targetId;
        }

        return null;
    }

    public MapSourceIdResult MapSourceId<T>(Expression<Func<T, object>> keyNameSelector, int? sourceId, bool useLocator = true)
    {
        if (sourceId == null) return new MapSourceIdResult(true, null);

        var memberName = keyNameSelector.GetMemberName();
        var fullKeyName = $"{typeof(T).FullName}.{memberName}.{sourceId}";
        if (sourceId == 0)
        {
            // TODO tk: 2022-05-31 source data autofix applied
            _logger.LogWarning("{key} Key locator invalid argument, cannot supply 0 as argument", fullKeyName);
            return new MapSourceIdResult(true, null);
        }
        
        if (GetExplicitMapping<T>(memberName, sourceId) is { } explicitlyMappedId)
        {
            _logger.LogTrace("{key} resolved as {value} from explicit mapping", fullKeyName, explicitlyMappedId);
            return new MapSourceIdResult(true, explicitlyMappedId);
        }

        if (_mappings.TryGetValue(fullKeyName, out var resultId))
        {
            _logger.LogTrace("{key} resolved as {value}", fullKeyName, resultId);
            return new MapSourceIdResult(true, resultId);
        }
        
        if (sourceId is not { } sid)
        {
            return new MapSourceIdResult(true, null);
        }
        
        _logger.LogTrace("TryLocate {key}", fullKeyName);
        if(useLocator && _primaryKeyLocatorService.TryLocate(keyNameSelector, sid, out var targetId))
        {
            SetMapping(keyNameSelector, sid, targetId); // cache id
            _logger.LogTrace("{key} located as {value}", fullKeyName, targetId);
            return new MapSourceIdResult(true, targetId);
        }

        return new MapSourceIdResult(false, null);
    }

    public void PreloadDependencies<T>(Expression<Func<T, object>> keyNameSelector)
    {
        foreach (var (sourceId, targetId) in _primaryKeyLocatorService.SelectAll(keyNameSelector))
        {
            SetMapping(keyNameSelector, sourceId, targetId);    
        }
    }
}