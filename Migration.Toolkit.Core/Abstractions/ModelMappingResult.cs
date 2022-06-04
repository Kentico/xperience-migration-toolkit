using System.Collections.Immutable;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Migration.Toolkit.Core.Abstractions;

public interface IModelMappingResult
{
    string Message { get; }
    bool Success { get; }
}

public interface IModelMappingResult<out TResult>: IModelMappingResult
{
    TResult? Item { get; }
    bool NewInstance { get; }
}

public record AggregatedResult<TResult>(TResult? Item, bool NewInstance) : IModelMappingResult<TResult>
{
    public IReadOnlyCollection<IModelMappingResult> Results { get; private set; } = new Collection<IModelMappingResult>();

    public string Message => string.Join(", ", this.Results);

    public bool Success => this.Results.All(x => x.Success);

    public void AddResult(IModelMappingResult result)
    {
        this.Results = this.Results.Concat(new[] { result }).ToImmutableList();
    }
}

public abstract record ModelMappingResult<TResult>(TResult? Item, bool Success, string Message, bool NewInstance) : IModelMappingResult<TResult>;

public record ModelMappingSuccess<TResult>(TResult? Result, bool NewInstance) : ModelMappingResult<TResult>(Result, true, null, NewInstance);

public record ModelMappingFailed<TResult>(string Message) : ModelMappingResult<TResult>(default, false, Message, false);
public record ModelMappingFailedKeyMismatch<TResult>() : ModelMappingResult<TResult>(default, false, $"Entity Guid mismatch, cannot map entity {typeof(TResult).FullName}", false);
public record ModelMappingFailedSourceNotDefined<TResult>() : ModelMappingResult<TResult>(default, false, $"Source entity is not defined for target {typeof(TResult).FullName}", false);