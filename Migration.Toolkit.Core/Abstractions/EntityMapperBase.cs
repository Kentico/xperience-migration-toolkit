using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Migration.Toolkit.Common;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.MigrationProtocol;

namespace Migration.Toolkit.Core.Abstractions;

public abstract class EntityMapperBase<TSourceEntity, TTargetEntity>: IEntityMapper<TSourceEntity, TTargetEntity>
{
    private readonly ILogger _logger;
    private readonly PrimaryKeyMappingContext _pkContext;
    private readonly IMigrationProtocol _protocol;
    
    protected EntityMapperBase(ILogger logger, PrimaryKeyMappingContext pkContext, IMigrationProtocol protocol)
    {
        _logger = logger;
        _pkContext = pkContext;
        _protocol = protocol;
    }

    public virtual IModelMappingResult<TTargetEntity> Map(TSourceEntity? source, TTargetEntity? target)
    {
        var failures = new List<IModelMappingResult<TTargetEntity>>();
        var mappingHelper = new MappingHelper(_pkContext, failures.Add);
        
        if (source is null)
        {
            _logger.LogTrace("Source entity is not defined.");
            return HandbookReferences.SourceEntityIsNull<TSourceEntity>().AsFailure<TTargetEntity>().Log(_logger, _protocol);
        }

        var newInstance = false;
        if (target is null)
        {
            _logger.LogTrace("Null target supplied, creating new instance.");
            target = CreateNewInstance(source, mappingHelper, failures.Add);
            if (target == null && object.Equals(target, default) && failures.Count > 0)
            {
                return new AggregatedResult<TTargetEntity>(failures).Log(_logger, _protocol);                
            }

            newInstance = true;
        }
        // else if (source.NodeGuid != target.NodeGUID)
        // {
        //     // assertion failed
        //     _logger.LogTrace("Assertion failed, entity key mismatch.");
        //     return new ModelMappingFailedKeyMismatch<TTargetEntity>().Log(_logger);
        // }
        
        
        var mappedResult = MapInternal(source, target, newInstance, mappingHelper, failures.Add);

        return failures.Count > 0 
            ? new AggregatedResult<TTargetEntity>(failures).Log(_logger, _protocol) 
            : new MapperResultSuccess<TTargetEntity>(mappedResult, newInstance).Log(_logger, _protocol);
    }

    protected abstract TTargetEntity? CreateNewInstance(TSourceEntity source, MappingHelper mappingHelper, AddFailure addFailure);
    protected abstract TTargetEntity MapInternal(TSourceEntity source, TTargetEntity target, bool newInstance, MappingHelper mappingHelper, AddFailure addFailure);

    protected delegate void AddFailure(MapperResultFailure<TTargetEntity> failure);

    protected class MappingHelper
    {
        private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;
        private readonly Action<IModelMappingResult<TTargetEntity>> _addFailure;

        public MappingHelper(PrimaryKeyMappingContext primaryKeyMappingContext, Action<IModelMappingResult<TTargetEntity>> addFailure)
        {
            _primaryKeyMappingContext = primaryKeyMappingContext;
            _addFailure = addFailure;
        }

        public Guid Require(Guid? value, string valueName)
        {
            if (value is Guid v && v != Guid.Empty)
            {
                return v;
            }

            var failure = HandbookReferences
                    .SourceValueIsRequired<TSourceEntity>(valueName)
                    .AsFailure<TTargetEntity>()
                ;    
                
            _addFailure(failure);
            return Guid.Empty;
        }
        
        public int Require(int? value, string valueName)
        {
            if (value is int v)
            {
                return v;
            }

            var failure = HandbookReferences
                    .SourceValueIsRequired<TSourceEntity>(valueName)
                    .AsFailure<TTargetEntity>()
                ;    
                
            _addFailure(failure);
            return -1;
        }
        
        public bool Require(bool? value, string valueName)
        {
            if (value is bool v)
            {
                return v;
            }

            var failure = HandbookReferences
                    .SourceValueIsRequired<TSourceEntity>(valueName)
                    .AsFailure<TTargetEntity>()
                ;    
                
            _addFailure(failure);
            return false;
        }
        
        public DateTime Require(DateTime? value, string valueName)
        {
            if (value is DateTime v)
            {
                return v;
            }

            var failure = HandbookReferences
                    .SourceValueIsRequired<TSourceEntity>(valueName)
                    .AsFailure<TTargetEntity>()
                ;    
                
            _addFailure(failure);
            return DateTime.MinValue;
        }

        public bool TranslateId<TKeyOwner>(Expression<Func<TKeyOwner, object>> keyNameSelector, int? sourceId, out int? translatedId)
        {
            if (sourceId == null)
            {
                translatedId = null;
                return true;
            }
            
            translatedId = _primaryKeyMappingContext.MapFromSourceOrNull(keyNameSelector, sourceId);
            if (sourceId.HasValue && !translatedId.HasValue)
            {
                var memberName = keyNameSelector.GetMemberName();
                var failure = HandbookReferences
                        .MissingRequiredDependency<TKeyOwner>(memberName, sourceId)
                        .NeedsManualAction()
                        .AsFailure<TTargetEntity>()
                    ;    
                
                _addFailure(failure);
                return false;
            }

            return true;
        }
        
        public bool TryTranslateId<TKeyOwner>(Expression<Func<TKeyOwner, object>> keyNameSelector, int? sourceId, out int? translatedId)
        {
            translatedId = _primaryKeyMappingContext.MapFromSourceOrNull(keyNameSelector, sourceId);
            if (sourceId.HasValue && !translatedId.HasValue)
            {
                return false;
            }
            
            return true;
        }

        public bool TranslateRequiredId<TKeyOwner>(Expression<Func<TKeyOwner, object>> keyNameSelector, int? sourceId, out int translatedId)
        {
            if (_primaryKeyMappingContext.TryRequireMapFromSource(keyNameSelector, sourceId, out translatedId))
            {
                return true;
            }

            var memberName = keyNameSelector.GetMemberName();
            var failure = HandbookReferences
                    .MissingRequiredDependency<TKeyOwner>(memberName, sourceId)
                    .NeedsManualAction()
                    .AsFailure<TTargetEntity>()
                ;    
            _addFailure(failure);
            return false;
        }
    }
}