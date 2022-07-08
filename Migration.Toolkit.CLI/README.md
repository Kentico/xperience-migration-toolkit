## CLI Usage

## Before you start

- Empty instance is required
    - no page instances
    - no contact instances
    - no contact activities
    - no form data
- Target instance must not be running

## How to use tool

1) prepare configuration [more here](#Configuration)
2) ensure target instance is not running
3) run tool with preferred command arguments [more here](#Command migrate)

## Command migrate

> warning: only empty sites are currently fully supported.

Command usage `migrate --sites --users`

| Parameter                   | Description                                              | Required parameters                      | Dependencies                         |
|-----------------------------|----------------------------------------------------------|------------------------------------------|--------------------------------------|
| `--siteId <siteId>`         | Required argument, specifies source SiteID for migration |                                          |                                      |
| `--sites`                   | Performs migration of Site objects                       | Configuration of explicit SiteID mapping | none                                 |
| `--users`                   | Performs migration of User objects                       | none                                     | `--sites`                            |
| `--contact-management`      | Performs migration of Contact groups                     | none                                     | `--users`                            |
| `--data-protection`         |                                                          |                                          | `--sites`, `--users`                 |
| `--forms`                   |                                                          |                                          | `--sites`                            |
| `--media-libraries`         |                                                          |                                          | `--sites`, `--users`                 |
| `--page-types`              |                                                          |                                          | `--sites`                            |
| `--pages`                   |                                                          | `--culture`                              | `--sites`, `--users`, `--page-types` |
| `--settings-keys`           |                                                          |                                          | `--sites`                            |
| `--culture <culture>`       |                                                          |                                          |                                      |
| `--bypass-dependency-check` | Tool will skip command dependency check                  |                                          |                                      |
| `--attachments`             | Performs migration of Attachments to media library       |                                          | `--sites`                            |

### Examples

1. `Migration.Toolkit.CLI.exe migrate --sites --users --settings-keys --media-libraries --page-types --pages --culture en-US`
2. `Migration.Toolkit.CLI.exe migrate --page-types --pages --culture en-US --bypass-dependency-check` - if you want to retry pages migration
   when `--sites` and `--users` migration was already performed

### Common behavior and errors during migration

#### --users

* administrator users will not be updated only created
* user with username `public` will be mapped to user `public` in target instance
* user roles are migrated
* user site relation are migrated

#### --sites

*

## Configuration

To run tool, configure `appsettings.json` file:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "System": "Warning",
      "Microsoft": "Warning"
    },
    "MinimumLevel": {
      "Default": "Information",
      "System": "Warning",
      "Microsoft": "Warning"
    },
    "pathFormat": "logs/log.txt"
  },
  "Settings": {
    "SourceConnectionString": "[TODO]",
    "SourceCmsDirPath": "[TODO]",
    "TargetConnectionString": "[TODO]",
    "TargetCmsDirPath": "[TODO]",
    "MigrateOnlyMediaFileInfo": false,
    "TargetKxoApiSettings": {
      "ConnectionStrings": {
        "CMSConnectionString": "[TODO]"
      }
    },
    "UseOmActivityNodeRelationAutofix": "AttemptFix",
    "UseOmActivitySiteRelationAutofix": "AttemptFix",
    "TargetAttachmentMediaLibraryName": "CmsAttachmentForSite{siteName}Or{siteId}",
    "EntityConfigurations": {
      "CMS_Site": {
        "ExplicitPrimaryKeyMapping": {
          "SiteID": {
            "1": 1
          }
        }
      },
      "CMS_Class": {
        "ExcludeCodeNames": [
          "CMS.File",
          "CMS.ImageGallery",
          "CMS.News",
          "CMS.MenuItem",
          "CMS.Article",
          "CMS.Blog",
          "CMS.Job",
          "CMS.Office",
          "CMS.BlogPost"
        ]
      },
      "CMS_SettingsKey": {
        "ExcludeCodeNames": [
          "CMSDataVersion",
          "CMSDBVersion",
          "CMSStoreAllowAnonymousCustomers",
          "CMSStoreInvoiceTemplate",
          "CMSStoreShowTaxRegistrationID",
          "CMSStoreShowOrganizationID",
          "CMSStoreUseExtraCompanyAddress",
          "CMSStoreCheckoutProcess",
          "CMSStoreRequireCompanyInfo",
          "CMSNewsletterUseExternalService",
          "CMSApplicationState",
          "CMSTranslationsLastStatusCheck",
          "CMSSalesForceLeadReplicationMappingDateTime",
          "CMSDBSeparationStartedByServer",
          "CMSStoreTransformationsInFS",
          "CMSStoreLayoutsInFS",
          "CMSStorePageTemplatesInFS",
          "CMSStoreWebPartLayoutsInFS",
          "CMSDeploymentMode",
          "CMSStoreWebpartContainersInFS",
          "CMSStoreAltFormLayoutsInFS",
          "CMSStoreFormLayoutsInFS",
          "CMSModuleUsageTrackingIdentity",
          "CMSEnableCI",
          "CMSAllowDynamicNewsletters",
          "CMSHotfixDataVersion",
          "CMSHotfixProcedureInProgress",
          "CMSRichTextEditorLicense",
          "CMSRESTUrlHashSalt",
          "CMSJwtTokenEncryptionKey"
        ]
      }
    }
  }
}
```

| Property path                                                           | Description                                                                                                          |
|-------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------|
| Settings.SourceConnectionString                                         | Source kentico instance connection string for tool usage                                                             |
| Settings.SourceCmsDirPath                                               | Source kentico instance filesystem absolute path - important if you want to migrate media files stored on filesystem |
| Settings.TargetConnectionString                                         | Target (KXO) instance connection string for tool usage                                                               |
| Settings.TargetCmsDirPath                                               | Target kentico instance filesystem absolute path - important if you want to migrate media files stored on filesystem |
| Settings.TargetKxoApiSettings                                           | KXO Api Settings - `ConnectionStrings.CMSConnectionString` is required                                               |
| Settings.EntityConfigurations.CMS_Site.ExplicitPrimaryKeyMapping.SiteID | Required - mapping of source siteId to target siteId (currently site creation is not supported)                      |
| Settings.MigrateOnlyMediaFileInfo                                       | if media files are stored on filesystem and not in cloud storage, set setting to `true`                              |
| Settings.TargetKxoApiSettings                                           | kentico api settings                                                                                                 |
| Settings.UseOmActivityNodeRelationAutofix                               | possible options: [`DiscardData`,`AttemptFix`,`Error`]                                                               |
| Settings.UseOmActivitySiteRelationAutofix                               | possible options: [`DiscardData`,`AttemptFix`,`Error`]                                                               |
| Settings.TargetAttachmentMediaLibraryName                               | name of library where Attachment object will be migrated, `{siteName}` and `{siteId}` macros can be used             |
| Settings.EntityConfigurations                                           | migration of some object can be fine-tuned using these options                                                       |

## User handbook

