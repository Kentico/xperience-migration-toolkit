using CMS.Base;
using CMS.DocumentEngine;
using CMS.Helpers;

namespace Migration.Toolkit.KXO.Api;

public record IsPublishedArgument(
    bool? DocumentCanBePublished,
    int? DocumentWorkflowStepId,
    bool? DocumentIsArchived,
    int? DocumentCheckedOutVersionHistoryId,
    int? DocumentPublishedVersionHistoryId,
    DateTime? DocumentPublishFrom,
    DateTime? DocumentPublishTo
);

public class KxoPageFacade
{
    public KxoPageFacade()
    {
        
    }

    public bool IsPublished(IsPublishedArgument arg)
    {
        // code copied from api:
        // DocumentHelper.GetPublished((IDataContainer)this);
        
        // /// <summary>
        // /// Gets the published state of the document from the given data container
        // /// </summary>
        // /// <param name="dc">Data container</param>
        // public static bool GetPublished(IDataContainer dc)
        // {
        //     if (dc.ContainsColumn("DocumentCanBePublished"))
        //     {
        //         if (!ValidationHelper.GetBoolean(dc.GetValue("DocumentCanBePublished"), false))
        //             return false;
        //     }
        //     else
        //     {
        //         foreach (string columnName in DocumentColumnLists.CANBEPUBLISHED_REQUIRED_COLUMNS)
        //         {
        //             if (!dc.ContainsColumn(columnName))
        //                 throw new Exception("[DocumentHelper.GetPublished]: There must be 'DocumentCanBePublished' or '" + columnName + "' column present in order to evaluate the published status of the document.");
        //         }
        //         if (dc.GetValue("DocumentWorkflowStepID") == null || ValidationHelper.GetBoolean(dc.GetValue("DocumentIsArchived"), false) || dc.GetValue("DocumentCheckedOutVersionHistoryID") != null != (dc.GetValue("DocumentPublishedVersionHistoryID") != null))
        //             return false;
        //     }
        //     if (!dc.ContainsColumn("DocumentPublishFrom"))
        //         throw new Exception("[DocumentHelper.GetPublished]: There must be 'DocumentPublishFrom' column present in order to evaluate the published status of the document.");
        //     if (!dc.ContainsColumn("DocumentPublishTo"))
        //         throw new Exception("[DocumentHelper.GetPublished]: There must be 'DocumentPublishTo' column present in order to evaluate the published status of the document.");
        //     return DateTime.Now >= ValidationHelper.GetDateTime(dc.GetValue("DocumentPublishFrom"), DateTime.MinValue) && DateTime.Now <= ValidationHelper.GetDateTime(dc.GetValue("DocumentPublishTo"), DateTime.MaxValue);
        // }

        var (documentCanBePublished, documentWorkflowStepId, documentIsArchived, documentCheckedOutVersionHistoryId, documentPublishedVersionHistoryId, documentPublishFrom, documentPublishTo) = arg;
        if (documentCanBePublished.HasValue)
        {
            if (!documentCanBePublished.Value)
                return false;
        }
        else
        {
            // foreach (string columnName in DocumentColumnLists.CANBEPUBLISHED_REQUIRED_COLUMNS)
            // {
            //     if (!dc.ContainsColumn(columnName))
            //         throw new Exception("[DocumentHelper.GetPublished]: There must be 'DocumentCanBePublished' or '" + columnName + "' column present in order to evaluate the published status of the document.");
            // }
            
            if (documentWorkflowStepId == null || documentIsArchived.GetValueOrDefault(false) || documentCheckedOutVersionHistoryId != null != (documentPublishedVersionHistoryId != null))
                return false;
        }
        // if (!dc.ContainsColumn("DocumentPublishFrom"))
        //     throw new Exception("[DocumentHelper.GetPublished]: There must be 'DocumentPublishFrom' column present in order to evaluate the published status of the document.");
        // if (!dc.ContainsColumn("DocumentPublishTo"))
        //     throw new Exception("[DocumentHelper.GetPublished]: There must be 'DocumentPublishTo' column present in order to evaluate the published status of the document.");
        return DateTime.Now >= ValidationHelper.GetDateTime(documentPublishFrom, DateTime.MinValue) && DateTime.Now <= ValidationHelper.GetDateTime(documentPublishTo, DateTime.MaxValue);
    }
}