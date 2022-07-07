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