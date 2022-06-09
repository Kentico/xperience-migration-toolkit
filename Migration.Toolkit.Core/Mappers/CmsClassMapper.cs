using CMS.DataEngine;
using Microsoft.Extensions.Logging;
using Migration.Toolkit.Common;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.Convertors;
using Migration.Toolkit.KXO.Models;

namespace Migration.Toolkit.Core.Mappers;

public class CmsClassMapper : IEntityMapper<KX13.Models.CmsClass, KXO.Models.CmsClass>, IEntityMapper<KX13.Models.CmsClass, DataClassInfo>
{
    private readonly ILogger<CmsClass> _logger;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;
    private readonly FormInfoDefinitionConvertor _formInfoDefinitionConvertor;

    public CmsClassMapper(ILogger<CmsClass> logger, PrimaryKeyMappingContext primaryKeyMappingContext, FormInfoDefinitionConvertor formInfoDefinitionConvertor)
    {
        _logger = logger;
        _primaryKeyMappingContext = primaryKeyMappingContext;
        _formInfoDefinitionConvertor = formInfoDefinitionConvertor;
    }
    
    public IModelMappingResult<CmsClass> Map(KX13.Models.CmsClass? source, CmsClass? target)
    {
        if (source is null)
        {
            _logger.LogTrace("Source entity is not defined.");
            return new ModelMappingFailedSourceNotDefined<CmsClass>().Log(_logger);
        }

        var newInstance = false;
        if (target is null)
        {
            _logger.LogTrace("Null target supplied, creating new instance.");
            target = new CmsClass();
            newInstance = true;
        }
        else if (source.ClassName != target.ClassName)
        {
            // assertion failed
            _logger.LogTrace("Assertion failed, entity Guid mismatch.");
            return new ModelMappingFailedKeyMismatch<CmsClass>().Log(_logger);
        }
        
        // target.ClassId = source.ClassId;
        // TODO tk: 2022-05-17: check assigned sites target.Sites
        
        target.ClassDisplayName = source.ClassDisplayName;
        target.ClassName = source.ClassName;
        target.ClassUsesVersioning = source.ClassUsesVersioning;
        target.ClassIsDocumentType = source.ClassIsDocumentType;
        target.ClassIsCoupledClass = source.ClassIsCoupledClass;
        target.ClassXmlSchema = _formInfoDefinitionConvertor.ConvertToKxo(source.ClassXmlSchema);
        target.ClassFormDefinition = _formInfoDefinitionConvertor.ConvertToKxo(source.ClassFormDefinition);
        target.ClassNodeNameSource = source.ClassNodeNameSource;
        target.ClassTableName = source.ClassTableName;
        // target.ClassFormLayout = source.ClassFormLayout;
        target.ClassShowAsSystemTable = source.ClassShowAsSystemTable;
        target.ClassUsePublishFromTo = source.ClassUsePublishFromTo;
        target.ClassShowTemplateSelection = source.ClassShowTemplateSelection;
        target.ClassIsMenuItemType = source.ClassIsMenuItemType;
        target.ClassNodeAliasSource = source.ClassNodeAliasSource;
        target.ClassLastModified = source.ClassLastModified;
        target.ClassGuid = source.ClassGuid;
        target.ClassIsProduct = source.ClassIsProduct;
        target.ClassIsCustomTable = source.ClassIsCustomTable;
        target.ClassShowColumns = source.ClassShowColumns;
        // TODO tk: 2022-05-30  Cannot set info object, domain validation failed (Field name: ClassSearchIndexDataSource)
        // target.ClassSearchIndexDataSource = source.ClassSearchIndexDataSource;
        // target.ClassSearchTitleColumn = source.ClassSearchTitleColumn;
        // target.ClassSearchContentColumn = source.ClassSearchContentColumn;
        // target.ClassSearchImageColumn = source.ClassSearchImageColumn;
        // target.ClassSearchCreationDateColumn = source.ClassSearchCreationDateColumn;
        // target.ClassSearchSettings = source.ClassSearchSettings;
        // target.ClassSearchEnabled = source.ClassSearchEnabled;
        
        // TODO tk: 2022-05-30 SKU disabled for now
        // target.ClassSkudefaultDepartmentName = source.ClassSkudefaultDepartmentName;
        // target.ClassSkudefaultDepartmentId = source.ClassSkudefaultDepartmentId;
        // target.ClassSkudefaultProductType = source.ClassSkudefaultProductType;
        // target.ClassSkumappings = source.ClassSkumappings;
        // target.ClassCreateSku = source.ClassCreateSku;
        
        target.ClassContactMapping = source.ClassContactMapping;
        target.ClassContactOverwriteEnabled = source.ClassContactOverwriteEnabled;
        target.ClassConnectionString = source.ClassConnectionString;
        target.ClassIsProductSection = source.ClassIsProductSection;
        // target.ClassFormLayoutType = source.ClassFormLayoutType;
        // target.ClassVersionGuid = source.ClassVersionGuid;
        target.ClassDefaultObjectType = source.ClassDefaultObjectType;
        target.ClassIsForm = source.ClassIsForm;
        target.ClassCustomizedColumns = source.ClassCustomizedColumns;
        target.ClassCodeGenerationSettings = source.ClassCodeGenerationSettings;
        target.ClassIconClass = source.ClassIconClass;
        target.ClassUrlpattern = source.ClassUrlpattern;
        target.ClassUsesPageBuilder = source.ClassUsesPageBuilder;
        target.ClassIsNavigationItem = source.ClassIsNavigationItem;
        target.ClassHasUrl = source.ClassHasUrl;
        target.ClassHasMetadata = source.ClassHasMetadata;

        target.ClassInheritsFromClassId = _primaryKeyMappingContext.MapFromSource<KX13.Models.CmsClass>(c => c.ClassId, source.ClassInheritsFromClassId);
        target.ClassResourceId = _primaryKeyMappingContext.MapFromSource<KX13.Models.CmsResource>(c => c.ResourceId, source.ClassResourceId);

        return new ModelMappingSuccess<CmsClass>(target, newInstance).Log(_logger);
    }

    public IModelMappingResult<DataClassInfo> Map(KX13.Models.CmsClass? source, DataClassInfo? target)
    {
        if (source is null)
        {
            _logger.LogTrace("Source entity is not defined.");
            return new ModelMappingFailedSourceNotDefined<DataClassInfo>().Log(_logger);
        }

        var newInstance = false;
        if (target is null)
        {
            _logger.LogTrace("Null target supplied, creating new instance.");
            target = DataClassInfo.New();
            newInstance = true;
        }
        else if (source.ClassName != target.ClassName)
        {
            // assertion failed
            _logger.LogTrace("Assertion failed, entity Guid mismatch.");
            return new ModelMappingFailedKeyMismatch<DataClassInfo>().Log(_logger);
        }
        
        // target.ClassId = source.ClassId;
        // TODO tk: 2022-05-17: check assigned sites target.Sites
        
        target.ClassDisplayName = source.ClassDisplayName;
        target.ClassName = source.ClassName;
        target.ClassUsesVersioning = source.ClassUsesVersioning;
        target.ClassIsDocumentType = source.ClassIsDocumentType;
        target.ClassIsCoupledClass = source.ClassIsCoupledClass;
        target.ClassXmlSchema = _formInfoDefinitionConvertor.ConvertToKxo(source.ClassXmlSchema);
        target.ClassFormDefinition = _formInfoDefinitionConvertor.ConvertToKxo(source.ClassFormDefinition);
        target.ClassNodeNameSource = source.ClassNodeNameSource;
        target.ClassTableName = source.ClassTableName;
        // TODO tk: 2022-06-07 check if convertible
        // target.ClassFormLayout = source.ClassFormLayout;
        target.ClassShowAsSystemTable = source.ClassShowAsSystemTable.UseKenticoDefault();
        target.ClassUsePublishFromTo = source.ClassUsePublishFromTo.UseKenticoDefault();
        target.ClassShowTemplateSelection = source.ClassShowTemplateSelection.UseKenticoDefault();
        target.ClassIsMenuItemType = source.ClassIsMenuItemType.UseKenticoDefault();
        target.ClassNodeAliasSource = source.ClassNodeAliasSource;
        target.ClassLastModified = source.ClassLastModified;
        target.ClassGUID = source.ClassGuid;
        target.ClassIsProduct = source.ClassIsProduct.UseKenticoDefault();
        target.ClassIsCustomTable = source.ClassIsCustomTable;
        target.ClassShowColumns = source.ClassShowColumns;
        target.ClassContactMapping = source.ClassContactMapping;
        target.ClassContactOverwriteEnabled = source.ClassContactOverwriteEnabled.UseKenticoDefault();
        target.ClassConnectionString = source.ClassConnectionString;
        target.ClassIsProductSection = source.ClassIsProductSection.UseKenticoDefault();
        // TODO tk: 2022-06-07 check if convertible
        // target.ClassFormLayoutType = source.ClassFormLayoutType.AsEnum<LayoutTypeEnum>();
        // TODO tk: 2022-06-07 check if convertible
        // target.ClassVersionGUID = source.ClassVersionGuid;
        target.ClassDefaultObjectType = source.ClassDefaultObjectType;
        target.ClassIsForm = source.ClassIsForm.UseKenticoDefault();
        target.ClassCustomizedColumns = source.ClassCustomizedColumns;
        target.ClassCodeGenerationSettings = source.ClassCodeGenerationSettings;
        target.ClassIconClass = source.ClassIconClass;
        target.ClassURLPattern = source.ClassUrlpattern;
        target.ClassUsesPageBuilder = source.ClassUsesPageBuilder;
        target.ClassIsNavigationItem = source.ClassIsNavigationItem;
        target.ClassHasURL = source.ClassHasUrl;
        target.ClassHasMetadata = source.ClassHasMetadata;
        
        // target.ClassSkumappings = source.ClassSkumappings;
        // target.ClassCreateSku = source.ClassCreateSku;
        // target.ClassSkudefaultDepartmentName = source.ClassSkudefaultDepartmentName;
        // target.ClassSKUDefaultDepartmentID = source.ClassSkudefaultDepartmentId;
        // target.ClassSKUDefaultProductType = source.ClassSkudefaultProductType;
        
        target.ClassInheritsFromClassID = _primaryKeyMappingContext.MapFromSource<KX13.Models.CmsClass>(c => c.ClassId, source.ClassInheritsFromClassId).UseKenticoDefault();
        target.ClassResourceID = _primaryKeyMappingContext.MapFromSource<KX13.Models.CmsResource>(c => c.ResourceId, source.ClassResourceId).UseKenticoDefault();

        // TODO tk: 2022-05-30  Cannot set info object, domain validation failed (Field name: ClassSearchIndexDataSource)
        // target.ClassSearchIndexDataSource = source.ClassSearchIndexDataSource.AsEnum<SearchIndexDataSourceEnum>();
        // target.ClassSearchEnabled = source.ClassSearchEnabled.UseKenticoDefault();
        // target.ClassSearchTitleColumn = source.ClassSearchTitleColumn;
        // target.ClassSearchContentColumn = source.ClassSearchContentColumn;
        // target.ClassSearchImageColumn = source.ClassSearchImageColumn;
        // target.ClassSearchCreationDateColumn = source.ClassSearchCreationDateColumn;
        // target.ClassSearchSettings = source.ClassSearchSettings;

        return new ModelMappingSuccess<DataClassInfo>(target, newInstance).Log(_logger);
    }
}