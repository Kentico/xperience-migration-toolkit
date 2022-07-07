using CMS.FormEngine;
using CMS.OnlineForms;
using Microsoft.Extensions.Logging;
using Migration.Toolkit.Common;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.MigrationProtocol;
using Migration.Toolkit.KXO.Models;

namespace Migration.Toolkit.Core.Mappers;

public class CmsFormMapper : EntityMapperBase<KX13.Models.CmsForm, BizFormInfo>
{
    public CmsFormMapper(
        ILogger<CmsFormMapper> logger,
        PrimaryKeyMappingContext primaryKeyMappingContext,
        IMigrationProtocol protocol
    ) : base(logger, primaryKeyMappingContext, protocol)
    {
    }

    protected override BizFormInfo? CreateNewInstance(KX13.Models.CmsForm source, MappingHelper mappingHelper, AddFailure addFailure)
    {
        var newBizFormInfo = BizFormInfo.New();
        newBizFormInfo.FormGUID = source.FormGuid;
        return newBizFormInfo;
    }

    protected override BizFormInfo MapInternal(KX13.Models.CmsForm source, BizFormInfo target, bool newInstance, MappingHelper mappingHelper, AddFailure addFailure)
    {
        // if (source.FormGuid != target.FormGUID)
        // {
        //     // assertion failed
        //     _logger.LogTrace("Assertion failed, entity key mismatch.");
        //     return new ModelMappingFailedKeyMismatch<BizFormInfo>().Log(_logger);
        // }

        // target.FormId = source.FormId;
        target.FormDisplayName = source.FormDisplayName;
        target.FormName = source.FormName;
        // target.FormSendToEmail = source.FormSendToEmail;
        // target.FormSendFromEmail = source.FormSendFromEmail;
        // target.FormEmailSubject = source.FormEmailSubject;
        // target.FormEmailTemplate = source.FormEmailTemplate;
        // target.FormEmailAttachUploadedDocs = source.FormEmailAttachUploadedDocs.UseKenticoDefault();
        target.FormItems = source.FormItems;
        target.FormReportFields = source.FormReportFields;
        target.FormSubmitButtonText = source.FormSubmitButtonText;
        // target.FormConfirmationEmailField = source.FormConfirmationEmailField;
        // target.FormConfirmationTemplate = source.FormConfirmationTemplate;
        // target.FormConfirmationSendFromEmail = source.FormConfirmationSendFromEmail;
        // target.FormConfirmationEmailSubject = source.FormConfirmationEmailSubject;
        target.FormAccess = source.FormAccess.AsEnum<FormAccessEnum>();
        target.FormSubmitButtonImage = source.FormSubmitButtonImage;
        target.FormLastModified = source.FormLastModified;
        target.FormLogActivity = source.FormLogActivity.UseKenticoDefault();
        target.FormBuilderLayout = source.FormBuilderLayout;

        // target.FormAfterSubmitMode = source.FormAfterSubmitMode;
        // target.FormAfterSubmitRelatedValue = source.FormAfterSubmitRelatedValue;

        // target.FormClassID = _primaryKeyMappingContext.RequireMapFromSource<KX13.Models.CmsClass>(c => c.ClassId, source.FormClassId);
        if (mappingHelper.TranslateRequiredId<KX13.Models.CmsClass>(c => c.ClassId, source.FormClassId, out var formClassId))
        {
            target.FormClassID = formClassId;
        }
        
        // target.FormSiteID = _primaryKeyMappingContext.RequireMapFromSource<KX13.Models.CmsSite>(c => c.SiteId, source.FormSiteId);
        if (mappingHelper.TranslateRequiredId<KX13.Models.CmsSite>(c => c.SiteId, source.FormSiteId, out var formSiteId))
        {
            target.FormSiteID = formSiteId;
        }

        return target;
    }
}

public class CmsFormMapperEf : EntityMapperBase<KX13.Models.CmsForm, KXO.Models.CmsForm>
{
    public CmsFormMapperEf(ILogger<CmsFormMapperEf> logger, PrimaryKeyMappingContext pkContext, IMigrationProtocol protocol) : base(logger, pkContext, protocol)
    {
    }

    protected override CmsForm? CreateNewInstance(KX13.Models.CmsForm source, MappingHelper mappingHelper, AddFailure addFailure) => new();

    protected override CmsForm MapInternal(KX13.Models.CmsForm source, CmsForm target, bool newInstance, MappingHelper mappingHelper, AddFailure addFailure)
    {
        // if (source.FormGuid != target.FormGuid)
        // {
        //     // assertion failed
        //     _logger.LogTrace("Assertion failed, entity key mismatch.");
        //     return new ModelMappingFailedKeyMismatch<KXO.Models.CmsForm>().Log(_logger);
        // }

        // target.FormId = source.FormId;
        target.FormDisplayName = source.FormDisplayName;
        target.FormName = source.FormName;
        // target.FormSendToEmail = source.FormSendToEmail;
        // target.FormSendFromEmail = source.FormSendFromEmail;
        // target.FormEmailSubject = source.FormEmailSubject;
        // target.FormEmailTemplate = source.FormEmailTemplate;
        // target.FormEmailAttachUploadedDocs = source.FormEmailAttachUploadedDocs;
        target.FormItems = source.FormItems;
        target.FormReportFields = source.FormReportFields;
        target.FormSubmitButtonText = source.FormSubmitButtonText;
        // target.FormConfirmationEmailField = source.FormConfirmationEmailField;
        // target.FormConfirmationTemplate = source.FormConfirmationTemplate;
        // target.FormConfirmationSendFromEmail = source.FormConfirmationSendFromEmail;
        // target.FormConfirmationEmailSubject = source.FormConfirmationEmailSubject;
        target.FormAccess = source.FormAccess;
        target.FormSubmitButtonImage = source.FormSubmitButtonImage;
        target.FormGuid = source.FormGuid;
        target.FormLastModified = source.FormLastModified;
        target.FormLogActivity = source.FormLogActivity;
        target.FormBuilderLayout = source.FormBuilderLayout;

        // TODO tk: 2022-05-20 new deduce: target.FormAfterSubmitMode = source.FormAfterSubmitMode;
        // TODO tk: 2022-05-20 new deduce: target.FormAfterSubmitRelatedValue = source.FormAfterSubmitRelatedValue;

        // target.FormClassId = _primaryKeyMappingContext.RequireMapFromSource<KX13.Models.CmsClass>(c => c.ClassId, source.FormClassId);
        if (mappingHelper.TranslateRequiredId<KX13.Models.CmsClass>(c => c.ClassId, source.FormClassId, out var classId))
        {
            target.FormClassId = classId;
        }
        // target.FormSiteId = _primaryKeyMappingContext.RequireMapFromSource<KX13.Models.CmsSite>(c => c.SiteId, source.FormSiteId);
        if (mappingHelper.TranslateRequiredId<KX13.Models.CmsSite>(c => c.SiteId, source.FormSiteId, out var siteId))
        {
            target.FormSiteId = siteId;
        }

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

        return target;
    }
}