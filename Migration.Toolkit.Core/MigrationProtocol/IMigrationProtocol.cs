﻿using MediatR;
using Migration.Toolkit.Core.Abstractions;

namespace Migration.Toolkit.Core.MigrationProtocol;

public interface IMigrationProtocol
{
    // TODO tk: 2022-05-20 whatNeedsToBeDoneOrWhatHappened will not be needed
    void NeedsManualAction<TSource, TTarget>(HandbookReference handbookRef, string whatNeedsToBeDoneOrWhatHappened, TSource source, TTarget? target, ModelMappingResult<TTarget> mapped);
    // TODO tk: 2022-05-20 whatNeedsToBeDoneOrWhatHappened will not be needed
    void NeedsManualAction<TSource, TTarget>(HandbookReference handbookRef, string whatNeedsToBeDoneOrWhatHappened, TSource source, TTarget? target);
    void MappedTarget<TTarget>(ModelMappingResult<TTarget> mapped);
    void FetchedTarget<TTarget>(TTarget? target);
    void FetchedSource<TSource>(TSource? source);
    void Success<TSource, TTarget>(TSource source, TTarget target, ModelMappingResult<TTarget> mapped);
    IDisposable CreateScope<TScopeType>();
    void Warning<T>(HandbookReference handbookRef, T? entity);
    void Warning<TSource, TTarget>(HandbookReference handbookRef, TSource? source, TTarget? target);
    void Fatal<T>(HandbookReference handbookRef, T? entity);
    
    void CommandRequest<TRequest, TResponse>(TRequest request) where TRequest : IRequest<TResponse>;
    void CommandFinished<TRequest, TResponse>(TRequest request, TResponse response) where TRequest : IRequest<TResponse> where TResponse : CommandResult;
    void CommandError<TRequest, TResponse>(Exception exception, TRequest request) where TRequest : IRequest<TResponse>;
    
}