using CMS.DataEngine;
using CMS.FormEngine;
using CMS.OnlineForms;
using Microsoft.Extensions.Logging;
using Migration.Toolkit.Common;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;

namespace Migration.Toolkit.Core.MigrateForms;

public class CmsFormMapper : IEntityMapper<KX13.Models.CmsForm, KXO.Models.CmsForm>, IEntityMapper<KX13.Models.CmsForm, BizFormInfo>
{
    private readonly ILogger<CmsFormMapper> _logger;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;

    public CmsFormMapper(
        ILogger<CmsFormMapper> logger,
        PrimaryKeyMappingContext primaryKeyMappingContext
    )
    {
        _logger = logger;
        _primaryKeyMappingContext = primaryKeyMappingContext;
    }

    public ModelMappingResult<KXO.Models.CmsForm> Map(KX13.Models.CmsForm? source, KXO.Models.CmsForm? target)
    {
        if (source is null)
        {
            _logger.LogTrace("Source entity is not defined.");
            return new ModelMappingFailedSourceNotDefined<KXO.Models.CmsForm>();
        }

        var newInstance = false;
        if (target is null)
        {
            _logger.LogTrace("Null target supplied, creating new instance.");
            target = new KXO.Models.CmsForm();
            newInstance = true;
        }
        else if (source.FormGuid != target.FormGuid)
        {
            // assertion failed
            _logger.LogTrace("Assertion failed, entity key mismatch.");
            return new ModelMappingFailedKeyMismatch<KXO.Models.CmsForm>();
        }

        // target.FormId = source.FormId;
        target.FormDisplayName = source.FormDisplayName;
        target.FormName = source.FormName;
        target.FormSendToEmail = source.FormSendToEmail;
        target.FormSendFromEmail = source.FormSendFromEmail;
        target.FormEmailSubject = source.FormEmailSubject;
        target.FormEmailTemplate = source.FormEmailTemplate;
        target.FormEmailAttachUploadedDocs = source.FormEmailAttachUploadedDocs;
        target.FormItems = source.FormItems;
        target.FormReportFields = source.FormReportFields;
        target.FormSubmitButtonText = source.FormSubmitButtonText;
        target.FormConfirmationEmailField = source.FormConfirmationEmailField;
        target.FormConfirmationTemplate = source.FormConfirmationTemplate;
        target.FormConfirmationSendFromEmail = source.FormConfirmationSendFromEmail;
        target.FormConfirmationEmailSubject = source.FormConfirmationEmailSubject;
        target.FormAccess = source.FormAccess;
        target.FormSubmitButtonImage = source.FormSubmitButtonImage;
        target.FormGuid = source.FormGuid;
        target.FormLastModified = source.FormLastModified;
        target.FormLogActivity = source.FormLogActivity;
        target.FormBuilderLayout = source.FormBuilderLayout;

        // TODO tk: 2022-05-20 new deduce: target.FormAfterSubmitMode = source.FormAfterSubmitMode;
        // TODO tk: 2022-05-20 new deduce: target.FormAfterSubmitRelatedValue = source.FormAfterSubmitRelatedValue;

        // TODO tk: 2022-05-20 form class migration required
        target.FormClassId = _primaryKeyMappingContext.RequireMapFromSource<KX13.Models.CmsClass>(c => c.ClassId, source.FormClassId);
        target.FormSiteId = _primaryKeyMappingContext.RequireMapFromSource<KX13.Models.CmsSite>(c => c.SiteId, source.FormSiteId);

        // [ForeignKey("FormClassId")]
        // [InverseProperty("CmsForms")]
        // public virtual CmsClass FormClass { get; set; } = null!;
        // [ForeignKey("FormSiteId")]
        // [InverseProperty("CmsForms")]
        // public virtual CmsSite FormSite { get; set; } = null!;
        //
        // [ForeignKey("FormId")]
        // [InverseProperty("Forms")]
        // public virtual ICollection<CmsRole> Roles { get; set; }

        return new ModelMappingSuccess<KXO.Models.CmsForm>(target, newInstance);
    }

    public ModelMappingResult<BizFormInfo> Map(KX13.Models.CmsForm? source, BizFormInfo? target)
    {
        if (source is null)
        {
            _logger.LogTrace("Source entity is not defined.");
            return new ModelMappingFailedSourceNotDefined<BizFormInfo>();
        }

        var newInstance = false;
        if (target is null)
        {
            _logger.LogTrace("Null target supplied, creating new instance.");
            target = BizFormInfo.New();
            target.FormGUID = source.FormGuid;
            newInstance = true;
        }
        else if (source.FormGuid != target.FormGUID)
        {
            // assertion failed
            _logger.LogTrace("Assertion failed, entity key mismatch.");
            return new ModelMappingFailedKeyMismatch<BizFormInfo>();
        }

        // target.FormId = source.FormId;
        target.FormDisplayName = source.FormDisplayName;
        target.FormName = source.FormName;
        target.FormSendToEmail = source.FormSendToEmail;
        target.FormSendFromEmail = source.FormSendFromEmail;
        target.FormEmailSubject = source.FormEmailSubject;
        target.FormEmailTemplate = source.FormEmailTemplate;
        target.FormEmailAttachUploadedDocs = source.FormEmailAttachUploadedDocs.UseKenticoDefault();
        target.FormItems = source.FormItems;
        target.FormReportFields = source.FormReportFields;
        target.FormSubmitButtonText = source.FormSubmitButtonText;
        target.FormConfirmationEmailField = source.FormConfirmationEmailField;
        target.FormConfirmationTemplate = source.FormConfirmationTemplate;
        target.FormConfirmationSendFromEmail = source.FormConfirmationSendFromEmail;
        target.FormConfirmationEmailSubject = source.FormConfirmationEmailSubject;
        target.FormAccess = source.FormAccess.AsEnum<FormAccessEnum>();
        target.FormSubmitButtonImage = source.FormSubmitButtonImage;
        target.FormLastModified = source.FormLastModified;
        target.FormLogActivity = source.FormLogActivity.UseKenticoDefault();
        target.FormBuilderLayout = source.FormBuilderLayout;

        // target.FormAfterSubmitMode = source.FormAfterSubmitMode;
        // target.FormAfterSubmitRelatedValue = source.FormAfterSubmitRelatedValue;

        // TODO tk: 2022-05-20 form class migration required
        target.FormClassID = _primaryKeyMappingContext.RequireMapFromSource<KX13.Models.CmsClass>(c => c.ClassId, source.FormClassId);
        target.FormSiteID = _primaryKeyMappingContext.RequireMapFromSource<KX13.Models.CmsSite>(c => c.SiteId, source.FormSiteId);

        // [ForeignKey("FormClassId")]
        // [InverseProperty("CmsForms")]
        // public virtual CmsClass FormClass { get; set; } = null!;
        // [ForeignKey("FormSiteId")]
        // [InverseProperty("CmsForms")]
        // public virtual CmsSite FormSite { get; set; } = null!;
        //
        // [ForeignKey("FormId")]
        // [InverseProperty("Forms")]
        // public virtual ICollection<CmsRole> Roles { get; set; }

        return new ModelMappingSuccess<BizFormInfo>(target, newInstance);
    }
}