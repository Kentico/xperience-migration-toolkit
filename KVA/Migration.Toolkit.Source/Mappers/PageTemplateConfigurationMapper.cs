namespace Migration.Toolkit.Source.Mappers;

using AngleSharp.Text;
using CMS.MediaLibrary;
using CMS.Websites;
using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.Extensions.Logging;
using Migration.Toolkit.Common.Abstractions;
using Migration.Toolkit.Common.Enumerations;
using Migration.Toolkit.Common.MigrationProtocol;
using Migration.Toolkit.Common.Services.Ipc;
using Migration.Toolkit.KXP.Api.Auxiliary;
using Migration.Toolkit.KXP.Api.Services.CmsClass;
using Migration.Toolkit.KXP.Models;
using Migration.Toolkit.Source.Contexts;
using Migration.Toolkit.Source.Model;
using Migration.Toolkit.Source.Services.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class PageTemplateConfigurationMapper(
    ILogger<PageTemplateConfigurationMapper> logger,
    PrimaryKeyMappingContext pkContext,
    IProtocol protocol,
    SourceInstanceContext sourceInstanceContext)
    : EntityMapperBase<ICmsPageTemplateConfiguration, PageTemplateConfigurationInfo>(logger, pkContext, protocol)
{
    protected override PageTemplateConfigurationInfo? CreateNewInstance(ICmsPageTemplateConfiguration source, MappingHelper mappingHelper, AddFailure addFailure)
        => source switch
        {
            CmsPageTemplateConfigurationK11 => null,
            CmsPageTemplateConfigurationK12 => PageTemplateConfigurationInfo.New(),
            CmsPageTemplateConfigurationK13 => PageTemplateConfigurationInfo.New(),
            _ => null
        };

    protected override PageTemplateConfigurationInfo MapInternal(ICmsPageTemplateConfiguration s, PageTemplateConfigurationInfo target,
        bool newInstance, MappingHelper mappingHelper, AddFailure addFailure)
    {
        if (s is ICmsPageTemplateConfigurationK12K13 source)
        {
            target.PageTemplateConfigurationDescription = source.PageTemplateConfigurationDescription;
            target.PageTemplateConfigurationName = source.PageTemplateConfigurationName;
            target.PageTemplateConfigurationLastModified = source.PageTemplateConfigurationLastModified;
            target.PageTemplateConfigurationIcon = "xp-custom-element"; // TODO tomas.krch: 2023-11-27 some better default icon pick?

            if (newInstance)
            {
                target.PageTemplateConfigurationGUID = source.PageTemplateConfigurationGUID;
            }

            if (sourceInstanceContext.HasInfo)
            {
                if (source.PageTemplateConfigurationTemplate != null)
                {
                    var pageTemplateConfiguration = JsonConvert.DeserializeObject<PageTemplateConfiguration>(source.PageTemplateConfigurationTemplate);
                    if (pageTemplateConfiguration?.Identifier != null)
                    {
                        logger.LogTrace("Walk page template configuration {Identifier}", pageTemplateConfiguration.Identifier);


                        var pageTemplateConfigurationFcs =
                            sourceInstanceContext.GetPageTemplateFormComponents(source.PageTemplateConfigurationSiteID, pageTemplateConfiguration.Identifier);
                        if (pageTemplateConfiguration.Properties is { Count: > 0 })
                        {
                            WalkProperties(pageTemplateConfiguration.Properties, pageTemplateConfigurationFcs);
                        }

                        target.PageTemplateConfigurationTemplate = JsonConvert.SerializeObject(pageTemplateConfiguration);
                    }
                }

                if (source.PageTemplateConfigurationWidgets != null)
                {
                    var areas = JsonConvert.DeserializeObject<Migration.Toolkit.Source.Services.Model.EditableAreasConfiguration>(source.PageTemplateConfigurationWidgets);
                    if (areas?.EditableAreas is { Count : > 0 })
                    {
                        WalkAreas(source.PageTemplateConfigurationSiteID, areas.EditableAreas);
                    }

                    target.PageTemplateConfigurationWidgets = JsonConvert.SerializeObject(areas);
                }
            }
            else
            {
                // simply copy if no info is available
                target.PageTemplateConfigurationTemplate = source.PageTemplateConfigurationTemplate;
                target.PageTemplateConfigurationWidgets = source.PageTemplateConfigurationWidgets;
            }

            return target;
        }
        else
        {
            return null!;
        }
    }

    #region "Page template & page widget walkers"

    private void WalkAreas(int siteId, List<Migration.Toolkit.Source.Services.Model.EditableAreaConfiguration> areas)
    {
        foreach (var area in areas)
        {
            logger.LogTrace("Walk area {Identifier}", area.Identifier);

            if (area.Sections is { Count: > 0 })
            {
                WalkSections(siteId, area.Sections);
            }
        }
    }

    private void WalkSections(int siteId, List<Migration.Toolkit.Source.Services.Model.SectionConfiguration> sections)
    {
        foreach (var section in sections)
        {
            logger.LogTrace("Walk section {TypeIdentifier}|{Identifier}", section.TypeIdentifier, section.Identifier);

            var sectionFcs = sourceInstanceContext.GetSectionFormComponents(siteId, section.TypeIdentifier);
            WalkProperties(section.Properties, sectionFcs);

            if (section.Zones is { Count: > 0 })
            {
                WalkZones(siteId, section.Zones);
            }
        }
    }

    private void WalkZones(int siteId, List<Migration.Toolkit.Source.Services.Model.ZoneConfiguration> zones)
    {
        foreach (var zone in zones)
        {
            logger.LogTrace("Walk zone {Name}|{Identifier}", zone.Name, zone.Identifier);

            if (zone.Widgets is { Count: > 0 })
            {
                WalkWidgets(siteId, zone.Widgets);
            }
        }
    }

    private void WalkWidgets(int siteId, List<Migration.Toolkit.Source.Services.Model.WidgetConfiguration> widgets)
    {
        foreach (var widget in widgets)
        {
            logger.LogTrace("Walk widget {TypeIdentifier}|{Identifier}", widget.TypeIdentifier, widget.Identifier);

            var widgetFcs = sourceInstanceContext.GetWidgetPropertyFormComponents(siteId, widget.TypeIdentifier);
            foreach (var variant in widget.Variants)
            {
                logger.LogTrace("Walk widget variant {Name}|{Identifier}", variant.Name, variant.Identifier);

                if (variant.Properties is { Count: > 0 })
                {
                    WalkProperties(variant.Properties, widgetFcs);
                }
            }
        }
    }

    private void WalkProperties(JObject? properties, List<EditingFormControlModel>? formControlModels)
    {
        if (properties == null) return;
        foreach (var (key, value) in properties)
        {
            logger.LogTrace("Walk property {Name}|{Identifier}", key, value?.ToString());

            var editingFcm = formControlModels?.FirstOrDefault(x => x.PropertyName?.Equals(key, StringComparison.InvariantCultureIgnoreCase) == true);
            if (editingFcm != null)
            {
                if (FieldMappingInstance.BuiltInModel.NotSupportedInKxpLegacyMode
                        .SingleOrDefault(x => x.OldFormComponent == editingFcm.FormComponentIdentifier) is var (oldFormComponent, newFormComponent))
                {
                    Protocol.Append(HandbookReferences.FormComponentNotSupportedInLegacyMode(oldFormComponent, newFormComponent));
                    logger.LogTrace("Editing form component found {FormComponentName} => no longer supported {Replacement}",
                        editingFcm.FormComponentIdentifier, newFormComponent);

                    switch (oldFormComponent)
                    {
                        case Kx13FormComponents.Kentico_AttachmentSelector when newFormComponent == FormComponents.AdminAssetSelectorComponent:
                        {
                            if (value?.ToObject<List<Migration.Toolkit.Source.Services.Model.AttachmentSelectorItem>>() is { Count: > 0 } items)
                            {
                                properties[key] = JToken.FromObject(items.Select(x => new AssetRelatedItem { Identifier = x.FileGuid }).ToList());
                            }

                            logger.LogTrace("Value migrated from {Old} model to {New} model", oldFormComponent, newFormComponent);
                            break;
                        }
                        case Kx13FormComponents.Kentico_PageSelector when newFormComponent == FormComponents.Kentico_Xperience_Admin_Websites_WebPageSelectorComponent:
                        {
                            if (value?.ToObject<List<Migration.Toolkit.Source.Services.Model.PageSelectorItem>>() is { Count: > 0 } items)
                            {
                                properties[key] = JToken.FromObject(items.Select(x => new WebPageRelatedItem { WebPageGuid = x.NodeGuid }).ToList());
                            }

                            logger.LogTrace("Value migrated from {Old} model to {New} model", oldFormComponent, newFormComponent);
                            break;
                        }
                        case Kx13FormComponents.Kentico_FileUploader:
                        {
                            break;
                        }
                    }
                }
                else
                {
                    // unknown control, probably custom
                    Protocol.Append(HandbookReferences.FormComponentCustom(editingFcm.FormComponentIdentifier!));
                    logger.LogTrace(
                        "Editing form component found {FormComponentName} => custom or inlined component, don't forget to migrate code accordingly",
                        editingFcm.FormComponentIdentifier);
                }
            }
        }
    }

    #endregion
}