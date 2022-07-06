using Migration.Toolkit.Core.MigrationProtocol;

namespace Migration.Toolkit.Core.Abstractions;

public interface IModelMappingResult
{
    bool Success { get; }
}

public interface IModelMappingResult<TResult>: IModelMappingResult
{
    TResult? Item { get; }
    bool NewInstance { get; }
    HandbookReference? HandbookReference { get; }
    
    void Deconstruct(out TResult? item, out bool newInstance)
    {
        item = this.Item;
        newInstance = this.NewInstance;
    }
    void Deconstruct(out HandbookReference? handbookReference)
    {
        handbookReference = this.HandbookReference;
    }
}

public record AggregatedResult<TResult>(IEnumerable<IModelMappingResult<TResult>> Results) : IModelMappingResult<TResult>
{
    public TResult? Item => throw new NotImplementedException();

    public bool NewInstance => throw new NotImplementedException();

    public HandbookReference? HandbookReference => throw new NotImplementedException();

    public bool Success => this.Results.All(x => x.Success);
}

public record MapperResult<TResult>(TResult? Item, bool NewInstance, bool Success, HandbookReference? HandbookReference): IModelMappingResult<TResult>;
public record MapperResultSuccess<TResult>(TResult? Item, bool NewInstance) : MapperResult<TResult>(Item, NewInstance, true, null);
public record MapperResultFailure<TResult>(HandbookReference HandbookReference) : MapperResult<TResult>(default, false, false, HandbookReference);

public static class Extensions
{
    public static IModelMappingResult<TResult> AsFailure<TResult>(this HandbookReference reference) => new MapperResultFailure<TResult>(reference);
}

//
// public abstract record ModelMappingResult<TResult>(TResult? Item, bool Success, string Message, bool NewInstance) : IModelMappingResult<TResult>
// {
//     public HandbookReference? HandbookReference { get; }
// }
//
// public record ModelMappingSuccess<TResult>(TResult? Result, bool NewInstance) : ModelMappingResult<TResult>(Result, true, null, NewInstance);
//
// public record ModelMappingFailed<TResult>(string Message) : ModelMappingResult<TResult>(default, false, Message, false);
// public record ModelMappingFailedKeyMismatch<TResult>() : ModelMappingResult<TResult>(default, false, $"Entity Guid mismatch, cannot map entity {typeof(TResult).FullName}", false);
// public record ModelMappingFailedSourceNotDefined<TResult>() : ModelMappingResult<TResult>(default, false, $"Source entity is not defined for target {typeof(TResult).FullName}", false);
// public record ModelMappingFailedMissingDependencyInTargetInstance<TResult>(string Name, object SourceId, HandbookReference Reference): ModelMappingResult<TResult>(default, false, $"Missing dependency in target instance for {typeof(TResult).FullName}", false);