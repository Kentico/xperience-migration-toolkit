namespace Migration.Toolkit.Core.Abstractions;

public interface IEntityMapper<TSourceEntity, TTargetEntity>
{
    IModelMappingResult<TTargetEntity> Map(TSourceEntity? source, TTargetEntity? target);
}