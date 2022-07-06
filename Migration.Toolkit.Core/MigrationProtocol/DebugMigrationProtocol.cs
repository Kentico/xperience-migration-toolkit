using MediatR;
using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;

namespace Migration.Toolkit.Core.MigrationProtocol;

public class DebugMigrationProtocol: IMigrationProtocol
{
    private readonly ILogger<DebugMigrationProtocol> _logger;

    public DebugMigrationProtocol(ILogger<DebugMigrationProtocol> logger)
    {
        _logger = logger;
    }

    public void NeedsManualAction<TData>(HandbookReference handbookRef, TData data)
    {
        _logger.LogDebug("NeedsManualAction<{dataType}>: {handBookRef} - {data}", typeof(TData).FullName, handbookRef, data);
    }

    public void NeedsManualAction<TSource, TTarget>(HandbookReference handbookRef, string whatNeedsToBeDoneOrWhatHappened, TSource source, TTarget? target, IModelMappingResult<TTarget> mapped)
    {
        _logger.LogDebug("NeedsManualAction<{sourceType}, {targetType}>: {handBookRef} - Mapped:{mapped}", typeof(TSource).FullName, typeof(TTarget), handbookRef, mapped);
    }
    
    public void NeedsManualAction<TSource, TTarget>(HandbookReference handbookRef, string whatNeedsToBeDoneOrWhatHappened, TSource source, TTarget? target)
    {
        _logger.LogDebug("NeedsManualAction<{sourceType}, {targetType}>: {handBookRef}", typeof(TSource).FullName, typeof(TTarget), handbookRef);
    }

    public void MappedTarget<TTarget>(IModelMappingResult<TTarget> mapped)
    {
        
    }

    public void FetchedTarget<TTarget>(TTarget? target)
    {
        _logger.LogDebug("FetchedTarget: {type}: {source}", typeof(TTarget).FullName, target);
    }

    public void FetchedSource<TSource>(TSource? source)
    {
        _logger.LogDebug("FetchedSource: {type}: {source}", typeof(TSource).FullName, source);
    }

    public void Success<TSource, TTarget>(TSource source, TTarget target, IModelMappingResult<TTarget> mapped)
    {
        
    }

    public IDisposable CreateScope<TScopeType>()
    {
        return new DummyDisposable();
    }
    
    private class DummyDisposable: IDisposable
    {
        public void Dispose() { }
    }

    public void Warning<T>(HandbookReference handbookRef, T? entity)
    {
        
    }

    public void Warning<TSource, TTarget>(HandbookReference handbookRef, TSource? source, TTarget? target)
    {
        
    }

    public void Fatal<T>(HandbookReference handbookRef, T? entity)
    {
        
    }

    public void Error<T>(HandbookReference handbookRef, T? entity)
    {
        _logger.LogDebug("Error: {handbookref}, entity: {entity}", handbookRef, entity);
    }

    public void CommandRequest<TRequest, TResponse>(TRequest request) where TRequest : IRequest<TResponse>
    {
        _logger.LogDebug("CommandRequest {request}", request);
    }

    public void CommandFinished<TRequest, TResponse>(TRequest request, TResponse response) where TRequest : IRequest<TResponse> where TResponse : CommandResult
    {
        _logger.LogDebug("CommandFinished {request} => {response}", request, response);
    }

    public void CommandError<TRequest, TResponse>(Exception exception, TRequest request) where TRequest : IRequest<TResponse>
    {
        _logger.LogDebug("CommandError {request} => {exception}", request, exception);
    }

    public void Append(HandbookReference? handbookReference)
    {
        _logger.LogDebug(handbookReference?.ToString());
    }
}