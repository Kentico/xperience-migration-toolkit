# Migration CLI

The [Xperience by Kentico: Kentico Migration Tool](/README.md) transfers content and other data from **Kentico Xperience 13**, **Kentico 12** or **Kentico 11** to **Xperience by Kentico**.

The migration is performed by running a command for the .NET CLI.

## Set up the target instance

The target of the migration must be an Xperience by Kentico instance that fulfills the following requirements:

* The instance's database and file system must be accessible from the environment where you run the migration.
* The target application *must not be running* when you start the migration.
* The target instance must be empty except for data from the source instance created by previous runs of this tool.
* For performance optimization, the migration transfers certain objects using bulk SQL queries. As a result, you always need to delete all objects of the following types before running repeated migrations:
  * **Contacts**, including their **Activities** (when using the `migrate --contact-management` parameter)
  * **Consent agreements** (when using the `migrate --data-protection` parameter)
  * **Form submissions** (when using the `migrate --forms` parameter)
  * **Custom module class data** (when using the `--custom-modules` parameter)

To create a suitable target instance, [install a new Xperience by Kentico project](https://docs.xperience.io/x/DQKQC) using the **Boilerplate** project template.

## Migrate data

To perform the migration:

  1. Make sure the [target instance is set up correctly](#set-up-the-target-instance).
  2. [Configure](#configuration) the options in the `Migration.Toolkit.CLI` project's `appsettings.json` file.
  3. Compile the `Migration.Toolkit.CLI` project.
  4. Open the command line prompt.
  5. Navigate to the project's output directory.
  6. Run the `Migration.Toolkit.CLI.exe migrate` command with parameters according to your requirements.
  7. Observe the command line output and review the [migration protocol](./MIGRATION_PROTOCOL_REFERENCE.md), which provides information about the result of the migration, lists required manual steps, etc.

### Migrate command parameters

Command usage:

```powershell
Migration.Toolkit.CLI.exe migrate --sites --custom-modules --users --members --forms --media-libraries --attachments --page-types --pages --settings-keys --contact-management --data-protection
```

| Parameter                   | Description                                              |  Dependencies                         |
|-----------------------------|----------------------------------------------------------|---------------------------------------|
| `--sites`                   | Enables migration of sites to [website channels](https://docs.xperience.io/x/34HFC). The site's basic properties and settings are transferred to the target instance.|  |
| `--custom-modules`          | Enables migration of custom modules, [custom module classes and their data](https://docs.xperience.io/x/AKDWCQ), and [custom fields in supported system classes](https://docs.xperience.io/x/V6rWCQ).<br /><br />See: [Migration details for specific object types - Custom modules and classes](#custom-modules-and-classes)  | `--sites` |
| `--custom-tables`          | Enables migration of [custom tables](https://docs.kentico.com/x/eQ2RBg).<br /><br />See: [Migration details for specific object types - Custom tables](#custom-tables)  |  |
| `--users`                   | Enables migration of [users](https://docs.xperience.io/x/8ILWCQ) and [roles](https://docs.xperience.io/x/7IVwCg).<br /><br />See: [Migration details for specific object types - Users](#users) | `--sites`, `--custom-modules` |
| `--members`                 | Enables migration of live site user accounts to [members](https://docs.xperience.io/x/BIsuCw). <br /><br />See: [Migration details for specific object types - Members](#members) | `--sites`, `--custom-modules` |
| `--settings-keys`           | Enables migration of values for [settings](https://docs.xperience.io/x/7YjFC) that are available in Xperience by Kentico. | `--sites`                             |
| `--page-types`              | Enables migration of [content types](https://docs.xperience.io/x/gYHWCQ) (originally *page types* in Kentico Xperience 13) and [preset page templates](https://docs.xperience.io/x/KZnWCQ) (originally *custom page templates*). Required to migrate Pages.<br /><br />See: [Migration details for specific object types - Content types](#content-types)  | `--sites`              |
| `--pages`                   | Enables migration of [pages](https://docs.xperience.io/x/bxzfBw).<br /><br />The target instance must not contain pages other than those created by previous runs of the Kentico Migration Tool.<br /><br />See: [Migration details for specific object types - Pages](#pages) | `--sites`, `--users`, `--page-types` |
| `--categories`              | Enables migration of categories to taxonomies. Xperience by Kentico uses a different approach to categorization. Categories are migrated to [taxonomies](https://docs.kentico.com/x/taxonomies_xp) and selected categories for each page are assigned to pages in the target instance via a [reusable field schema](https://docs.kentico.com/x/D4_OD). See [`Categories`](#categories). | `--sites`, `--users`, `--pagetypes`, `--pages` |
| `--attachments`             | Enables migration of page attachments to [media libraries](https://docs.xperience.io/x/agKiCQ) (page attachments are not supported in Xperience by Kentico).<br /><br />See: [Migration details for specific object types - Attachments](#attachments)   | `--sites`, `--custom-modules`  |
| `--contact-management`      | Enables migration of [contacts](https://docs.xperience.io/x/nYPWCQ) and [activities](https://docs.xperience.io/x/oYPWCQ). The target instance must not contain any contacts or activities. May run for a long time depending on the number of contacts in the source database. | `--users`, `--custom-modules` |
| `--data-protection`         | Enables migration of [consents](https://docs.xperience.io/x/zoB1CQ) and consent agreements.   | `--sites`, `--users`, `--contact management`  |
| `--forms`                   | Enables migration of [forms](https://docs.xperience.io/x/WAKiCQ) and submitted form data.<br /><br />See: [Migration details for specific object types - Forms](#forms)  | `--sites`, `--custom-modules`, `--users`  |
| `--media-libraries`         | Enables migration of [media libraries](https://docs.xperience.io/x/agKiCQ) and contained media files. The actual binary files are only migrated between file systems if the `MigrateOnlyMediaFileInfo` [configuration option](#configuration) is set to *false*. | `--sites`, `--custom-modules`, `--users` |
| `--countries`               | Enables migration of countries and states. Xperience by Kentico currently uses countries and states to fill selectors when editing contacts and contact group conditions. |  |
| `--bypass-dependency-check` | Skips the migrate command's dependency check. Use for repeated runs of the migration if you know that dependencies were already migrated successfully (for example `--page types` when migrating pages).  |  |

### Examples

* `Migration.Toolkit.CLI.exe migrate --sites --custom-modules --users --settings-keys --media-libraries --page-types --pages`
  * First migration that includes the site object, custom modules and classes, users, setting key values, media libraries, page types and pages
* `Migration.Toolkit.CLI.exe migrate --page-types --pages --bypass-dependency-check`
  * Repeated migration only for page types and pages, if you know that sites and users were already migrated successfully.
* `Migration.Toolkit.CLI.exe migrate --pages --bypass-dependency-check`
  * Repeated migration only for pages, if you know that page types, sites, and users were already migrated successfully.

### Migration details for specific object types

#### Content types

Content types are named **Page types** in earlier Kentico products.

Xperience by Kentico currently does not support:

* Macro expressions in page type field default values or other settings. Content type fields containing macros will not work correctly after the migration.
* Page type inheritance. You cannot migrate page types that inherit fields from other types.
* Categories for page type fields. Field categories are not migrated with page types.

The Kentico Migration Tool attempts to map the *Data type* and *Form control* of page type fields to an appropriate equivalent in Xperience by Kentico. This is not always possible, and cannot be done for custom data types or form controls. We recommend that you check your content type fields after the migration and adjust them if necessary.

The following table describes how the Kentico Migration Tool maps the data types and form controls/components of page type fields:

| KX13/12/11 Data type      | XbK Data type            | KX13/12/11 Form control       | XbK Form component    |
| ------------------------- | ------------------------ | ----------------------------- | --------------------- |
| Text                      | Text                     | Text box                      | Text input            |
| Text                      | Text                     | Drop-down list                | Dropdown selector     |
| Text                      | Text                     | Radio buttons                 | Radio button group    |
| Text                      | Text                     | Text area                     | Text area             |
| Text                      | Text                     | *other*                       | Text input            |
| Long text                 | Long text                | Rich text editor              | Rich text editor      |
| Long text                 | Long text                | Text box                      | Text input            |
| Long text                 | Long text                | Drop-down list                | Dropdown selector     |
| Long text                 | Long text                | Text area                     | Text area             |
| Long text                 | Long text                | *other*                       | Rich text editor      |
| Integer number            | Integer number           | *any*                         | Number input          |
| Long integer number       | Long integer number      | *any*                         | Number input          |
| Floating-point number     | Floating-point number    | *any*                         | Number input          |
| Decimal number            | Decimal number           | *any*                         | Decimal number input  |
| Date and time             | Date and time            | *any*                         | Datetime input        |
| Date                      | Date                     | *any*                         | Date input            |
| Time interval             | Time interval            | *any*                         | None (not supported)  |
| Boolean (Yes/No)          | Boolean (Yes/No)         | *any*                         | Checkbox              |
| Attachments               | Media files              | *any* (Attachments)           | Media file selector<br />(the [attachments](#attachments) are converted to media files)    |
| File                      | Media files              | *any* (Direct uploader)       | Media file selector<br />(the [attachments](#attachments) are converted to media files)    |
| Unique identifier (Guid)  | Unique identifier (Guid) | *any*                         | None (not supported)  |
| Pages                     | Pages                    | *any* (Pages)                 | Page selector         |

Additionally, you can enable the Conversion of text fields with media links (*Media selection* form control) to media library files by setting the `OptInFeatures.CustomMigration.FieldMigrations` [configuration option](#convert-text-fields-with-media-links-to-media-libraries).

Some [Form components](https://docs.xperience.io/x/5ASiCQ) used by content type fields in Xperience by Kentico store data differently than their equivalent Form control in Xperience 13. To ensure that content is displayed correctly on pages, you must manually adjust your website's implementation to match the new data format. See [Editing components in Xperience by Kentico](https://docs.xperience.io/x/wIfWCQ) to learn more about some of the most common components and selectors.

#### Reusable field schemas

You can create [reusable field schemas](https://docs.kentico.com/x/D4_OD) from page types from which other page types inherit, by setting the `Settings.CreateReusableFieldSchemaForClasses` [configuration option](#convert-page-types-to-reusable-field-schemas).

#### Pages

* The migration includes the following versions of pages:
  * _Published_
  * _Latest draft version_ - for published pages, the version is migrated to the _Draft_ [workflow step](https://docs.xperience.io/x/JwKQC#Pages-Pageworkflow); for pages that do not have a published version, the version is migrated to the _Draft (initial)_ workflow step.
  * _Archived_
* URLs are migrated depending on the source instance version:
  * For Kentico Xperience 13, the migration:
      * includes the URL paths of pages and Former URLs
      * does not include Alternative URLs
  * For Kentico 12 and Kentico 11, URL paths are not migrated. Instead, a default URL path is created from the `DocumentUrlPath` or `NodeAliasPath`.
* Linked pages are currently not supported in Xperience by Kentico. The migration creates standard page copies for any linked pages on the source instance.
* Page permissions (ACLs) are currently not supported in Xperience by Kentico and are not migrated.
* Migration of Page Builder content is only available for Kentico Xperience 13.

#### Page Builder content

> :warning: Page Builder content migration is only available when migrating from Kentico Xperience 13.

By default, JSON data storing the Page Builder content of pages and custom page templates is migrated directly without modifications. On the target Xperience by Kentico instance, the migrated data can work in the Page Builder's legacy compatibility mode. However, we strongly recommend updating your codebase to the new Xperience by Kentico components.

The Kentico Migration Tool provides an advanced migration mode for Page Builder content that utilizes API discovery on the source instance. To learn more details and how to configure this feature, see [Source instance API discovery](#source-instance-api-discovery).

#### Categories

Xperience by Kentico uses a different approach to categorization than older Kentico versions. [Categories](https://docs.kentico.com/13/configuring-xperience/configuring-the-environment-for-content-editors/configuring-categories) were replaced by [taxonomies](https://docs.kentico.com/developers-and-admins/configuration/taxonomies) and selected categories for each page are assigned to pages in the target instance via a [reusable field schema](https://docs.kentico.com/x/D4_OD). The key differences are:

* Categories in older versions can be added to any page via the *Properties -> Categories* tab. Taxonomies can only be used for content items (pages, emails...) that have a field with the *Taxonomy* data type.
* Categories can be global or site-specific. Taxonomies are always global, as there are no sites in Xperience by Kentico.
* Categories are assigned to pages regardless of their workflow step. Taxonomies are stored as a field and are covered by workflow. As a result, assigned tags can be different in each workflow step.
* [Categories stored as a field](https://docs.kentico.com/x/wA_RBg) and [personal categories](https://docs.kentico.com/x/IgqRBg) are not supported by the migration.

The migration process for categories performs the following steps:

1. A new [taxonomy](https://docs.kentico.com/developers-and-admins/configuration/taxonomies) named **Categories** (code name `categories`) is created to house all categories from the source instance.
2. A new [reusable field schema](https://docs.kentico.com/x/D4_OD) named **Categories container** (code name `categories_container`) is created to allow linking tags to pages.
  * The schema contains one field, **Categories_Legacy** (data type **Taxonomy**, configured to enable selection from the *Categories* taxonomy).
3. On the target instance, the *Categories container* reusable field schema is added to all content types where at least one page had a category assigned in the source instance.
4. Supported categories from the source instance are migrated as tags to the *Categories* taxonomy in the target instance. The category hierarchy from the source instance is maintained in the target instance.
5. On the target instance, tags are assigned to pages according to the source instance.
  * Each [language variant](https://docs.kentico.com/business-users/website-content/translate-pages) of a page is treated individually and receives its corresponding group of tags based on the source instance.
  * Tags from the source page are added to all available [workflow steps](https://docs.kentico.com/developers-and-admins/configuration/workflows) of the target page.

#### Custom modules and classes

The migration includes the following:

* Custom modules
  * Note: The `CMS.` prefix/namespace is reserved for system modules and not allowed in custom module code names. If present, this code name prefix is removed during the migration.
* All classes belonging under custom modules
* All data stored within custom module classes
* The following customizable system classes and their custom fields:
  * *Membership > User*
  * *Media libraries > Media file*
  * *Contact management > Contact management - Account* (however, accounts are currently not supported in Xperience by Kentico)
  * *Contact management > Contact management - Contact*

Module and class migration does NOT include:

* UI elements and all related user interface settings. The administration of Xperience by Kentico uses a different technology stack than Kentico Xperience 13 and is incompatible. To learn how to build the administration UI, see [Extend the administration interface](https://docs.xperience.io/x/GwKQC) and [Example - Offices management application](https://docs.xperience.io/x/hIFwCg).
* Alternative forms under classes and UI-related configuration of class fields (field labels, Form controls, etc.). You need to manually create appropriate [UI forms](https://docs.xperience.io/x/V6rWCQ) in Xperience by Kentico after the migration.
* Custom settings under modules, which are currently not supported in Xperience by Kentico
* Module permissions (permissions work differently in Xperience by Kentico, see [Role management](https://docs.xperience.io/x/7IVwCg) and [UI page permission checks](https://docs.xperience.io/x/8IKyCg))

As with all object types, the Kentico Migration Tool does not transfer code files to the target project. You need to manually move all code files generated for your custom classes (*Info*, *InfoProvider*, etc.).

To learn more about custom modules and classes in Xperience by Kentico, see the [Object types](https://docs.xperience.io/x/AKDWCQ) documentation.

#### Custom tables

The migration includes the following:

* Basic information about custom tables (from the `CMS_Class` table) is migrated to the custom module table (`CMS_Resource`) as a special `customtables` resource.
* Content of individual custom tables is migrated as module classes.

Custom table migration does NOT include:

* Any other data related to custom tables (queries, alternative forms) are discarded by the migration.
* UI elements related to custom tables such as listings and filters are not migrated and need to be implemented. The administration of Xperience by Kentico uses a different technology stack than Kentico Xperience 13 and is incompatible. To learn how to build the administration UI, see [Extend the administration interface](https://docs.xperience.io/x/GwKQC) and [Example - Offices management application](https://docs.xperience.io/x/hIFwCg).

#### Media libraries

* In Xperience by Kentico, Media libraries are global instead of site-specific.
* The code name of each media library on the target instance is `{SiteName}_{LibraryCodeName}`.
* Media library permissions are currently not supported in Xperience by Kentic and are not migrated.

#### Attachments

Page attachments are not supported in Xperience by Kentico. Attachment files are instead migrated into [media libraries](https://docs.xperience.io/x/agKiCQ).

* Page attachments are migrated into a media library named: *"Attachments for site \<sitename\>"*
* The media library contains folders matching the content tree structure for all pages with attachments (including empty folders for parent pages without attachments). The folders are named after the *node alias* of the source pages.
  * Each page's folder directly contains all unsorted attachments (files added on the *Attachments* tab in the source's *Pages* application).
  * Attachments stored in specific page fields are placed into subfolders, named in format: *"__fieldname"*. These subfolders can include multiple files for fields of the *Attachments* type, or a single file for *File* type fields.
* Any "floating" attachments without an associated page are migrated into the media library root folder.
* The migration does not include temporary attachments (created when a file upload is not finished correctly). If any are present on the source instance, a warning is logged in the [migration protocol](./MIGRATION_PROTOCOL_REFERENCE.md).

The following is an example of a media library created by the Kentico Migration Tool for page attachments:

#### Media library "Attachments for site DancingGoat"

* **Articles** (empty parent folder)
  * **Coffee-processing-techniques** (contains any unsorted attachments of the '/Articles/Coffee-processing-techniques' page)
    * **__Teaser** (contains attachments stored in the page's 'Teaser' field)
  * **Which-brewing-fits-you**
    * **__Teaser**
  * ...

Additionally, any attachments placed into the content of migrated pages **will no longer work** in Xperience by Kentico. This includes images and file download links that use **/getattachment** and **/getimage** URLs.

If you wish to continue using these legacy attachment URLs from earlier Kentico versions, you need to add a custom handler to your Xperience by Kentico project. See [`Migration.Toolkit.KXP.Extensions/README.MD`](/Migration.Toolkit.KXP.Extensions/README.MD) for instructions.

#### Forms

The migration does not include the content of form autoresponder and notification emails.

You can migrate form autoresponders to Xperience by Kentico manually by copying your HTML code and content into Email templates and Emails. See [Emails](https://docs.xperience.io/x/IaDWCQ).

#### Users

**Note**: Xperience by Kentico uses separate entities for users with access to the administration interface (*CMS_User* table) and live site visitor accounts (*CMS_Member* table). Consequently, only users whose *Privilege level* is set to *Editor* and above are migrated (*Users* -> edit a user -> *General* tab) via the `--users` command. To migrate live site accounts as well, use [`--members`](#migrate-command-parameters).

The command migrates all users with access to the administration interface. Note the following expected behavior:

* The 'administrator' user account is only transferred from the source if it does not exist on the target instance.
* The 'public' system user is updated, and all bindings (e.g., the site binding) are mapped automatically on the target instance.
* Site bindings are updated automatically for all migrated users.
* Users in Xperience by Kentico must have an email address. Migration is only supported for users with a **unique** email address on the source instance.
  * If you encounter issues related to email validation, you can change the default validation behavior via the `CMSEmailValidationRegex` [application key](https://docs.xperience.io/x/yA6RBg).
* Custom user fields can be migrated together with *module classes*.

Additionally, the command migrates all roles and user-role bindings for users whose *Privilege level* is *Editor* or higher.

Because Xperience by Kentico uses a different [permission model](https://docs.xperience.io/x/7IVwCg), no existing role permissions or UI personalization settings are migrated. After the migration, the permissions for each role must be configured again.

#### Members

In Xperience by Kentico, live site users are represented using a separate **Member** entity and stored in the *CMS_Member* table.

The migration identifies live site users as those without access to the administration interface. That is, only those accounts whose *Privilege level* is set to *None* (Users -> edit a user -> General tab) are migrated.

The migration includes:

* All system fields from the *CMS_User* and *CMS_UserSettings* tables. You can customize which fields are migrated via the `MemberIncludeUserSystemFields` configuration option. See [configuration](#configuration).
* All custom fields added to the *CMS_User* and *CMS_UserSettings* tables are migrated under `CMS_Member`.
  > If you are migrating custom fields, the `--custom-modules` migration command must be run before the `--members` command. For example:

  ```powershell
  Migration.Toolkit.CLI.exe migrate --sites --custom-modules  --users --members
  ```

The migration ***DOES NOT*** include:

* External login information associated with each account (e.g., Google or Facebook logins).
* User password hashes from the `CMS_User.UserPassword` column.

  After the migration, the corresponding `CMS_Member.MemberPassword` in the target Xperience by Kentico instance is `NULL`. This means that the migrated accounts **CANNOT** be used to sign in to the system under any circumstances. The account owners must first reset their password via ASP.NET Identity.

  See [Forms authentication](https://docs.xperience.io/x/t4ouCw) for a sample password reset process that can be adapted for this scenario. The general flow consists of these steps:

    1. Select the migrated member accounts.

        ```csharp
        // Selects members whose password is null and who don't use external providers to sign in
        var migratedMembers =
                MemberInfo.Provider
                        .Get()
                        .WhereNull("MemberPassword")
                        .WhereEquals("MemberIsExternal", 0);
        ```

    2. Generate password reset tokens for each account using `UserManager.GeneratePasswordResetTokenAsync(member)`.
    3. Send the password reset email to each account using `IEmailService`.

        ```csharp
        await emailService
                .SendEmail(new EmailMessage()
                {
                    Recipients = member.Email,
                    Subject = "Password reset request",
                    // {resetURL} targets a controller action with the password reset form
                    Body = $"To reset your account's password, click <a href=\"{resetUrl}\">here</a>."
                });
        ```

#### Contacts

* Custom contact fields can be migrated together with *module classes*.
* For performance reasons, contacts and related objects are migrated using bulk SQL queries. As a result, you always need to delete all Contacts, Activities and Consent agreements before running the migration (when using the `migrate --contact-management` parameter).

## Configuration

Before you run the migration, configure options in the `Migration.Toolkit.CLI/appsettings.json` file.

Add the options under the `Settings` section in the configuration file.

| Configuration                                            | Description                                                                                                          |
|----------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------|
| KxConnectionString                                       | The connection string to the source Kentico Xperience 13 database.                                                            |
| KxCmsDirPath                                             | The absolute file system path of the **CMS** folder in the source Kentico Xperience 13 administration project. Required to migrate media library files. |
| XbKConnectionString                                      | The connection string to the target Xperience by Kentico database.                                                               |
| XbKDirPath                                               | The absolute file system path of the root of the target Xperience by Kentico project. Required to migrate media library and page attachment files. |
| XbKApiSettings                                           | Configuration options set for the API when creating migrated objects in the target application.<br /><br />The `ConnectionStrings.CMSConnectionString`option is required - set the connection string to the target Xperience by Kentico database (the same value as `XbKConnectionString`).              |
| MigrationProtocolPath                                    | The absolute file system path of the location where the [migration protocol file](./MIGRATION_PROTOCOL_REFERENCE.md) is generated.<br /><br />For example: `"C:\\Logs\\Migration.Toolkit.Protocol.log"`               |
| MigrateOnlyMediaFileInfo                                 | If set to `true`, only the database representations of media files are migrated, without the files in the media folder in the project's file system. For example, enable this option if your media library files are mapped to a shared directory or Cloud storage.<br /><br />If `false`, media files are migrated based on the `KxCmsDirPath` location.  |
| MemberIncludeUserSystemFields | Determines which system fields from the *CMS_User* and *CMS_UserSettings* tables are migrated to *CMS_Member* in Xperience by Kentico. Fields that do not exist in *CMS_Member* are automatically created. <br /><br />The sample `appsettings.json` file included with the tool by default includes all user fields that can be migrated from Kentico Xperience 13. Exclude specific fields from the migration by removing them from this configuration option. |
| UseOmActivityNodeRelationAutofix                         | Determines how the migration handles references from Contact management activities to non-existing pages.<br /><br />Possible options:<br />`DiscardData` - faulty references are removed,<br />`AttemptFix` - references are updated to the IDs of corresponding pages created by the migration,<br />`Error` - an error is reported and the reference can be translated or otherwise handled manually |
| UseOmActivitySiteRelationAutofix                         | Determines how the migration handles site references from Contact management activities.<br /><br />Possible options: `DiscardData`,`AttemptFix`,`Error` |
| EntityConfigurations                                           | Contains options that allow you to fine-tune the migration of specific object types.                 |
| EntityConfigurations.*&lt;object table name&gt;*.ExcludeCodeNames      | Excludes objects with the specified code names from the migration.                                   |
| CreateReusableFieldSchemaForClasses | Specifies which page types are also converted to [reusable field schemas](#convert-page-types-to-reusable-field-schemas). |
| OptInFeatures.QuerySourceInstanceApi.Enabled                   | If `true`, [source instance API discovery](#source-instance-api-discovery) is enabled to allow advanced migration of Page Builder content for pages and page templates. |
| OptInFeatures.QuerySourceInstanceApi.Connections               | To use [source instance API discovery](#source-instance-api-discovery), you need to add a connection JSON object containing the following values:<br />`SourceInstanceUri` - the base URI where the source instance's live site application is running.<br />`Secret` - the secret that you set in the *ToolkitApiController.cs* file on the source instance.  |
| OptInFeatures.CustomMigration.FieldMigrations                  | Enables conversion of media selection text fields to media library files. See [Convert text fields with media links to media libraries](#convert-text-fields-with-media-links-to-media-libraries) for more information.|

### Example

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
    "KxConnectionString": "Data Source=myserver;Initial Catalog=Xperience13;Integrated Security=True;Persist Security Info=False;Connect Timeout=120;Encrypt=False;Current Language=English;",
    "KxCmsDirPath": "C:\\inetpub\\wwwroot\\Xperience13\\CMS",
    "XbKConnectionString": "Data Source=myserver;Initial Catalog=XperienceByKentico;Integrated Security=True;Persist Security Info=False;Connect Timeout=120;Encrypt=False;Current Language=English;",
    "XbKDirPath": "C:\\inetpub\\wwwroot\\XP_Target",
    "XbKApiSettings": {
      "ConnectionStrings": {
        "CMSConnectionString": "Data Source=myserver;Initial Catalog=XperienceByKentico;Integrated Security=True;Persist Security Info=False;Connect Timeout=120;Encrypt=False;Current Language=English;"
      }
    },
 "MigrationProtocolPath": "C:\\_Development\\xperience-migration-toolkit-master\\Migration.Toolkit.Protocol.log",
 "MemberIncludeUserSystemFields": "FirstName|MiddleName|LastName|FullName|UserPrivilegeLevel|UserIsExternal|LastLogon|UserLastModified|UserGender|UserDateOfBirth",
 "MigrateOnlyMediaFileInfo": false,
    "UseOmActivityNodeRelationAutofix": "AttemptFix",
    "UseOmActivitySiteRelationAutofix": "AttemptFix",
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
          "CMS.MenuItem",
          "ACME.News",
          "ACME.Office",
          "CMS.Blog",
          "CMS.BlogPost"
        ]
      },
      "CMS_SettingsKey": {
        "ExcludeCodeNames": [
          "CMSHomePagePath"
        ]
      }
    },
    "OptInFeatures":{
        "QuerySourceInstanceApi": {
          "Enabled": true,
          "Connections": [
            { "SourceInstanceUri": "http://localhost:60527", "Secret": "__your secret string__" }
          ]
        },
        "FieldMigrations": {
            "SourceDataType": "text",
            "TargetDataType": "assets",
            "SourceFormControl": "MediaSelectionControl",
            "TargetFormComponent": "Kentico.Administration.AssetSelector",
            "Actions": [ "convert to asset" ],
            "FieldNameRegex": ".*"
        }
    }
  }
}
```

## Source instance API discovery

> :warning: **Warning** – source instance API discovery is only available when migrating from Kentico Xperience 13.

By default, JSON data storing the Page Builder content of pages and custom page templates is migrated directly without modifications. Within this content, Page Builder components (widgets, sections, etc.) with properties have their configuration based on Kentico Xperience 13 form components, which are assigned to the properties on the source instance. On the target Xperience by Kentico instance, the migrated data can work in the Page Builder's legacy compatibility mode.

However, we strongly recommend updating your codebase to the new Xperience by Kentico components. See [Editing components in Xperience by Kentico](https://docs.xperience.io/x/wIfWCQ) to learn more.

To convert Page Builder data to a format suitable for the Xperience by Kentico components, the Kentico Migration Tool provides an advanced migration mode that utilizes API discovery on the source instance. The advanced mode currently provides the following data conversion:

* **Attachment selector** properties - converted to a format suitable for the Xperience by Kentico **Media selector** component, with `IEnumerable<AssetRelatedItem>` values.
* **Page selector** properties - converted to a format suitable for the Xperience by Kentico Page selector component, with `IEnumerable<WebPageRelatedItem>` values.

### Prerequisites and Limitations

* To use source instance API discovery, the live site application of your source instance must be running and available during the migration.
* Using the advanced Page Builder data migration **prevents the data from being used in the Page Builder's legacy compatibility mode**. With this approach, you need to update all Page Builder component code files to the [Xperience by Kentico format](https://docs.xperience.io/x/wIfWCQ).
* The source instance API discovery feature only processes component properties defined using `[EditingComponent]` attribute notation. Other implementations, such as properties edited via custom view components in the Razer view, are not supported.

```csharp
public class MyWidgetProperties : IWidgetProperties
{
    // Supported
    [EditingComponent(PageSelector.IDENTIFIER, Label = "Selected products", Order = 1)]
    public IEnumerable<PageSelectorItem> SelectedProducts { get; set; } = new List<PageSelectorItem>();

    // NOT supported
    public IEnumerable<MediaFilesSelectorItem> Images { get; set; } = new List<MediaFilesSelectorItem>();
}
```

### API discovery setup

1. Copy the `ToolkitApiController.cs` file to the `Controllers` folder in the **live site project** of your Kentico Xperience 13 source instance. Get the file from the following location in the Kentico Migration Tool repository:

    * For .NET Core projects: `KX13.Extensions\ToolkitApiController.cs`
    * For MVC 5 (.NET Framework 4.8) projects: `KX13.NET48.Extensions\ToolkitApiController.NET48.cs`

2. Register routes for the `ToolkitApi` controller's actions into the source instance's live site application.

    * For .NET Core projects, add endpoints in the project's `Startup.cs` or `Program.cs` file:

      ```csharp
      app.UseEndpoints(endpoints =>
      {
          endpoints.MapControllerRoute(
              name: "ToolkitExtendedFeatures",
              pattern: "{controller}/{action}",
              constraints: new
              {
                  controller = "ToolkitApi"
              }
          );

          // other routes ...
      });
      ```

    * For MVC 5 projects, map the routes in your application's `RouteCollection` (e.g., in the `/App_Start/RouteConfig.cs` file):

      ```csharp
      public static void RegisterRoutes(RouteCollection routes)
      {
          // Maps routes for Xperience handlers and enabled features
          routes.Kentico().MapRoutes()

          routes.MapRoute(
              name: "ToolkitExtendedFeatures",
              url: "{controller}/{action}",
              defaults: new { },
              constraints: new
              {
                  controller = "ToolkitApi"
              }
          );

          // other routes ...
      }
      ```

3. Edit `ToolkitApiController.cs` and set a value for the `Secret` constant:

    ```csharp
    private const string Secret = "__your secret string__";
    ```

4. Configure the `Settings.OptInFeatures.QuerySourceInstanceApi` [configuration options](#configuration) for the Migration Tool:

    ```json
    "OptInFeatures":{
      "QuerySourceInstanceApi": {
        "Enabled": true,
        "Connections": [
          { "SourceInstanceUri": "http://localhost:60527", "Secret": "__your secret string__" }
        ]
      }
    },
    ```

You can test the source instance API discovery by making a POST request to `<source instance live site URI>/ToolkitApi/Test` with `{ "secret":"__your secret string__" }` in the body. If your setup is correct, the response should be: `{ "pong": true }`

When you now [migrate data](#migrate-data), the tool performs API discovery of Page Builder component code on the source instance and advanced migration of Page Builder data.

## Convert page types to reusable field schemas

It is not possible to migrate any page types that inherit fields from other page types. However, to make the manual migration of such page types easier, you can create [reusable field schemas](https://docs.kentico.com/x/D4_OD) from specified parent page types. Specify a list of page types to be converted to reusable field schemas (separated with either `;` or `,`) in the `Settings.CreateReusableFieldSchemaForClasses` [configuration option](#configuration).

The following example specifies two page types from which reusable schemas are created:

```json
"Settings":{
  ...

  "CreateReusableFieldSchemaForClasses": "Acme.SeoFields;Acme.ArticleFields"
},
```

> :warning: **Notes**
>
> * Conversion of page types to reusable field schemas works best when all field names of page types are unique (i.e., prefixed with the page type name). If multiple page types converted to reusable field schemas have fields with the same code name, the code name is prefixed with the content type name in the converted reusable field schemas.
> * Page types specified by this configuration option are also migrated as content types into to the target instance.

## Convert text fields with media links to media libraries

By default, page type and module class fields with the _Text_ data type and the _Media selection_ [form control](https://docs.xperience.io/x/0A_RBg) from the source instance are converted to plain _Text_ fields in the target instance. You can instead configure the Kentico Migration Tool to convert these fields to the _Media files_ data type and use the _Media file selector_ form component.

* Attachment links (containing a `getattachment` handler) are migrated as [attachments](#attachments) and changed to the _Media files_ data type.
* Media file links (containing a `getmedia` handler) are changed to the _Media files_ data type. It is expected that the media library containing the targeted file has been migrated.

> :warning: **Notes**
>
> * Only media libraries using the **Permanent** [file URL format](https://docs.xperience.io/x/xQ_RBg) are supported. Content from media libraries with enabled **Use direct path for files in content** setting will not be converted.
> * If you enable this feature, you also need to change retrieval and handling of affected files in your code, as the structure of the stored data changes from a text path (e.g.,`~/getmedia/CCEAD0F0-E2BF-459B-814A-36699E5C773E/somefile.jpeg?width=300&height=100`) to a _Media files_ data type (internally stored as e.g., `[{"Identifier":"CCEAD0F0-E2BF-459B-814A-36699E5C773E","Some file":"somefile.jpeg","Size":11803,"Dimensions":{"Width":300,"Height":100}}]`). The value of the field now needs to be [retrieved as a media library file](https://docs.xperience.io/x/LA2RBg).

To enable this feature, configure the `OptInFeatures.CustomMigration.FieldMigrations` [options](#configuration) for this tool. Use the values in the code snippet below:

```json
"OptInFeatures":{
  "CustomMigration":{
    "FieldMigrations": [
      {
        "SourceDataType": "text",
        "TargetDataType": "assets",
        "SourceFormControl": "MediaSelectionControl",
        "TargetFormComponent": "Kentico.Administration.AssetSelector",
        "Actions": [ "convert to asset" ],
        "FieldNameRegex": ".*"
      }
    ]
  }
}
```

`FieldNameRegex` - a regular expression used to filter what fields are converted. Only fields with field names that match the regular expressions are converted. Use `.*` to match all fields.
