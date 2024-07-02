[//]: # "[![Contributors][contributors-shield]][contributors-url]"
[//]: # "[![Forks][forks-shield]][forks-url]"
[//]: # "[![Stargazers][stars-shield]][stars-url]"
[//]: # "[![Issues][issues-shield]][issues-url]"
[//]: # "[![MIT License][license-shield]][license-url]"
[//]: # "[![Discord][discussion-shield]][discussion-url]"

<!-- ABOUT THE PROJECT -->
# Xperience by Kentico: Kentico Migration Tool

[![7-day bug-fix policy](https://img.shields.io/badge/-7--days_bug--fixing_policy-grey?labelColor=orange&logo=data:image/svg+xml;base64,PHN2ZyBjbGFzcz0ic3ZnLWljb24iIHN0eWxlPSJ3aWR0aDogMWVtOyBoZWlnaHQ6IDFlbTt2ZXJ0aWNhbC1hbGlnbjogbWlkZGxlO2ZpbGw6IGN1cnJlbnRDb2xvcjtvdmVyZmxvdzogaGlkZGVuOyIgdmlld0JveD0iMCAwIDEwMjQgMTAyNCIgdmVyc2lvbj0iMS4xIiB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciPjxwYXRoIGQ9Ik04ODguNDkgMjIyLjY4NnYtMzEuNTRsLTY1LjY3Mi0wLjk1NWgtMC4yMDVhNDY1LjcxNSA0NjUuNzE1IDAgMCAxLTE0NC4zMTUtMzEuMzM0Yy03Ny4wMDUtMzEuMTk4LTEyNi4yOTQtNjYuNzY1LTEyNi43MDMtNjcuMTA3bC0zOS44LTI4LjY3Mi0zOS4xODUgMjguNDY4Yy0yLjA0OCAxLjUwMS00OS45MDMgMzYuMDQ0LTEyNi45MDggNjcuMzFhNDQ3LjQyIDQ0Ny40MiAwIDAgMS0xNDQuNTIgMzEuMzM1bC02NS44NzcgMC45NTZ2Mzc4Ljg4YzAgODcuMDQgNDkuODM0IDE4NC42NjEgMTM3LjAxIDI2Ny44MSAzNy41NDcgMzUuODQgNzkuMjU4IDY2LjM1NSAxMjAuODMzIDg4LjIgNDMuMjggMjIuNzMzIDg0LjI0IDM0LjYxMiAxMTguODUyIDM0LjYxMiAzNC40MDYgMCA3NS43NzYtMTIuMTUyIDExOS42MDMtMzUuMTU4YTU0Ny45NzcgNTQ3Ljk3NyAwIDAgMCAxMjAuMDEzLTg3LjY1NCA1MTUuMjA5IDUxNS4yMDkgMCAwIDAgOTYuMTg4LTEyMi44OGMyNy4xMDItNDkuNTYyIDQwLjgyMy05OC4zMDQgNDAuODIzLTE0NC45OTlsLTAuMTM2LTM0Ny4yMDR6TTUxMC4wOSAxNDMuNDI4bDEuNzA2LTEuMzY1IDEuNzc1IDEuMzY1YzUuODAzIDQuMTY1IDU5LjUyOSA0MS44NDggMTQwLjM1NiA3NC43NTIgNzkuMTkgMzIuMDg2IDE1My42IDM1LjYzNSAxNjcuNjYzIDM2LjA0NWwyLjU5NCAwLjA2OCAwLjIwNSAzMTUuNzM0YzAuMTM3IDY5LjQ5NS00Mi41OTggMTUwLjE4Ni0xMTcuMDc3IDIyMS40NTdDNjQxLjU3IDg1NC4yODkgNTYzLjEzIDg5Ni40NzggNTEyIDg5Ni40NzhjLTIzLjY4OSAwLTU1LjU3LTkuODk5LTg5LjcwMi0yNy43ODVhNDc4LjgyMiA0NzguODIyIDAgMCAxLTEwNS42MDktNzcuMjc4QzI0Mi4yMSA3MjAuMjEzIDE5OS40NzUgNjM5LjUyMiAxOTkuNDc1IDU2OS44OVYyNTQuMjI1bDIuNzMtMC4xMzZjMy4yNzggMCA4Mi42MDQtMS41MDIgMTY3LjY2NC0zNS45NzdhNzM5Ljk0MiA3MzkuOTQyIDAgMCAwIDE0MC4yMi03NC42MTV2LTAuMDY5eiIgIC8+PHBhdGggZD0iTTcxMy4zMTggMzY4LjY0YTMyLjIyMiAzMi4yMjIgMCAwIDAtNDUuMzI5IDBMNDQ5LjE5NSA1ODcuNDM1bC05My4xODQtOTMuMTE2YTMyLjIyMiAzMi4yMjIgMCAwIDAtNDUuMzMgMCAzMi4yMjIgMzIuMjIyIDAgMCAwIDAgNDUuMjZsMTE1Ljg1IDExNS44NWEzMi4yOSAzMi4yOSAwIDAgMCA0NS4zMjggMEw3MTMuMzIgNDEzLjlhMzIuMjIyIDMyLjIyMiAwIDAgMCAwLTQ1LjMzeiIgIC8+PC9zdmc+)](https://github.com/Kentico/.github/blob/main/SUPPORT.md#full-support)

## Description

The Kentico Migration Tool transfers content and other data from **Kentico Xperience 13**, **Kentico 12** or **Kentico 11** to **Xperience by Kentico**.

## Prerequisites & Compatibility

### Source

The migration currently supports the Kentico Xperience 13, Kentico 12 or Kentico 11 as the source instance. See the following sections for compatibility information and limitations of respective versions.

#### Kentico Xperience 13

* The source of the migration data must be a Kentico Xperience 13 instance, with **Refresh 5** ([hotfix](https://devnet.kentico.com/download/hotfixes) 13.0.64) or newer applied.
* The development model (Core or MVC 5) does not affect the migration - both are supported.
* The source instance's database and file system must be accessible from the environment where you run the Kentico Migration Tool.
* All features described in this repository are available for migration from Kentico Xperience 13.

#### Kentico 12

* The source of the migration data can be any hotfix version of the Kentico 12.
* Only MVC development model is supported by this tool. Any Portal Engine project that you wish to migrate to Xperience by Kentico needs to be [migrated to MVC](https://www.youtube.com/watch?v=g2oeHU0h1e0) first.
* The source instance's database and file system must be accessible from the environment where you run the this tool.
* Migration of Page Builder content is not supported. Only structured data of pages is migrated.
  * As a result, [source instance API discovery](/Migration.Toolkit.CLI/README.md#source-instance-api-discovery) is also not available.
* This repository describes the migration of Kentico Xperience 13 feature set. Only features relevant to Kentico 12 are migrated for this version.

#### Kentico 11

* The source of the migration data can be any hotfix version of the Kentico 11. If you encounter any issues, it is recommended to update to the latest hotfix.
* Only MVC development model is supported by this tool. Any Portal Engine project that you wish to migrate to Xperience by Kentico needs to be [migrated to MVC](https://www.youtube.com/watch?v=g2oeHU0h1e0) first.
* The source instance's database and file system must be accessible from the environment where you run this tool.
* Only structured data of pages is migrated as Page Builder is not present in Kentico 11.
  * As a result, [source instance API discovery](/Migration.Toolkit.CLI/README.md#source-instance-api-discovery) is also not available.
* This repository describes the migration of Kentico Xperience 13 feature set. Only features relevant to Kentico 11 are migrated for this version.

### Target

* The Kentico Migration Tool is periodically updated to support migration to the **latest version** of Xperience by Kentico. However, there may be delays between Xperience by Kentico releases and tool updates.
  * Currently, Xperience by Kentico **29.1.0** is tested and supported.
* The target instance's database and file system must be accessible from the environment where you run this tool.
* The target instance's database must be empty except for data from the source instance created by previous runs of this tool to avoid conflicts and inconsistencies.

## Supported data and limitations

The Kentico Migration Tool does not transfer all data available in the source instance. Xperience by Kentico currently provides a smaller, more focused set of features. As a result, some objects are discarded or migrated to a suitable alternative.

This tool only supports content and objects **stored in the database** and related binary data on the file system, such as media library files. Code, customizations, and any other types of content need to be migrated manually to the target project and adjusted for Xperience by Kentico.

Currently, the Kentico Migration Tool supports the following types of data:

* **Sites**
  * The tool migrates each site on the source to a [website channel](https://docs.kentico.com/x/34HFC) object in Xperience by Kentico.
* **Cultures**
  * The set of cultures used across all sites in the source gets mapped to a [language](https://docs.kentico.com/x/OxT_Cw) in the _Languages_ application.
* **Content types** (_Page types_ in earlier Kentico versions)
  * The Kentico Migration Tool attempts to map the _Data type_ and _Form control_ of page type fields to an appropriate equivalent in Xperience by Kentico. This mapping is not always possible and does not work for custom data types or form controls. We recommend checking your content type fields after the migration and adjusting them if necessary.
  * The migration includes only page types assigned to at least one site on the source instance.
  * Xperience by Kentico currently does not support:
    * Macro expressions in page type field default values or other settings. Content type fields containing macros will not work correctly after the migration.
    * Categories for page type fields. Field categories are not migrated with page types.
    * Page type inheritance. Page types that inherit fields are migrated including all inherited fields but the binding to the parent page type is not preserved.
      * However, you can create [reusable field schemas](./Migration.Toolkit.CLI/README.md#convert-page-types-to-reusable-field-schemas) for page types from which other page types inherit.
  * All migrated Content types have the **Include in routing** option enabled (the migration never creates pages without URL and routing).
* **Pages**
  * The migration includes the following versions of pages:
    * _Published_
    * _Latest draft version_ - for published pages, the version is migrated to the _Draft_ [workflow step](https://docs.kentico.com/x/JwKQC); for pages that do not have a published version, the version is migrated to the _Draft (initial)_ workflow step.
    * _Archived_
  * Each page gets assigned under its corresponding website channel.
  * Linked pages are currently not supported in Xperience by Kentico. The migration creates standard page copies for any linked pages on the source instance.
  * Page permissions (ACLs) are currently not supported in Xperience by Kentico and are not migrated.
  * Migration of Page Builder content is only available for Kentico Xperience 13.
* **Page attachments**
  * Page attachments are not supported in Xperience by Kentico. Attachments are migrated into media libraries. See [`Migration.Toolkit.CLI/README.md - Attachments`](./Migration.Toolkit.CLI/README.md#Attachments) for detailed information about the conversion process.
* **Preset page templates** (_Custom page templates_ in Kentico Xperience 13)
  * Migration of custom page templates is only available for Kentico Xperience 13.
* **Categories**
  * Xperience by Kentico uses a different approach to categorization. Categories are migrated to [taxonomies](https://docs.kentico.com/x/taxonomies_xp) and selected categories for each page are assigned to pages in the target instance via a [reusable field schema](https://docs.kentico.com/x/D4_OD). See [`Migration.Toolkit.CLI/README.md - Categories`](./Migration.Toolkit.CLI/README.md#categories).
  * [Categories stored as a field of pages](https://docs.kentico.com/x/wA_RBg) and [personal categories](https://docs.kentico.com/x/IgqRBg) are not supported.
* **Media libraries and media files**
  * Media library permissions are currently not supported in Xperience by Kentico and are not migrated.
* **Forms**
  * The migration does not include the content of form autoresponder and notification emails. You can migrate form autoresponders to Xperience by Kentico manually by copying your HTML code and content into Email templates and Emails. See [Emails](https://docs.kentico.com/x/IaDWCQ).
* **Users**
  * Xperience by Kentico uses separate entities for users with access to the administration interface (_CMS\_User_ table) and live site visitor accounts (_CMS\_Member_ table). Consequently, only users whose _Privilege level_ is _Editor_ or higher are migrated (_Users_ -> edit a user -> _General_ tab).
  * Users in Xperience by Kentico must have an email address. Migration is only supported for users who have a unique email address value on the source instance.
  * Custom user fields are an optional part of _module class_ migration.
  * Live site users are represented using a separate **Member** entity and stored in the _CMS_Member_ table. The migration identifies live site users as those without access to the administration interface - accounts with _Privilege level_ set to _None_ (Users -> edit a user -> General tab).
* **Roles**
  * Only roles that have at least one user whose _Privilege level_ is set to _Editor_ and above are migrated.
  * Because Xperience by Kentico uses a different [permission model](https://docs.kentico.com/x/7IVwCg), no existing role permissions or UI personalization settings are migrated. After the migration, the permissions for each role must be configured again.
* **Contacts**
  * The target instance's _OMContact_ and _OMActivity_ database tables must be empty for performance reasons.
  * Custom contact fields are an optional part of _module class_ migration.
* **Activities**
* **Consents and consent agreements**
* **Modules and classes**
  * The migration includes the following:
    * Custom modules
    * All classes associated with custom modules
    * All data stored within custom module classes
    * The following customizable system classes and their custom fields: _User_, _Media file_, _Contact management - Account_ (however, accounts are currently not supported in Xperience by Kentico), _Contact management - Contact_
  * Module and class migration does NOT include:
    * UI elements and all related user interface settings. The administration of Xperience by Kentico uses a different technology stack than earlier Kentico versions and is incompatible. To learn how to build the administration UI, see [Extend the administration interface](https://docs.kentico.com/x/GwKQC) and [Example - Offices management application](https://docs.kentico.com/x/hIFwCg).
    * Alternative forms under classes and UI-related configuration of class fields (field labels, Form controls, etc.). After the migration, you must manually create the appropriate [UI forms](https://docs.kentico.com/x/V6rWCQ) in Xperience by Kentico.
    * Custom settings under modules, which are currently not supported in Xperience by Kentico
    * Module permissions (permissions work differently in Xperience by Kentico - see [Role management](https://docs.kentico.com/x/7IVwCg) and [UI page permission checks](https://docs.kentico.com/x/8IKyCg))
    * As with all object types, the Kentico Migration Tool does not transfer code files to the target project. You must manually move all code files generated for your custom classes (_Info_, _InfoProvider_, etc.).
* **Custom tables**
  * Custom tables are not supported in Xperience by Kentico. Data from custom tables is migrated to the target instance as custom modules.
  * The migration only transfers data from custom tables to the custom module (_CMS\_Resource_) database table.
  * Any user interface, listings, filters, and other functionality related to migrated custom tables needs to be implemented in the target instance.
* **Setting values**
  * The migration only transfers the settings that exist in Xperience by Kentico.
  * The migration excludes site-specific settings that do not have a corresponding website channel-specific alternative in Xperience by Kentico.

* **Countries and states**

### Unsupported data

The following types of data exist in Xperience by Kentico but are currently **not supported** by the Kentico Migration Tool:

* **Contact groups**
  * Static contact groups are currently not supported in Xperience by Kentico.
  * The condition format for dynamic contact groups is not compatible. To migrate contact groups:
    1. Migrate your contacts using the tool.
    2. Create the [contact groups](https://docs.kentico.com/x/o4PWCQ) manually in Xperience by Kentico.
    3. Build equivalent conditions.
    4. Recalculate the contact groups.
* **License keys**
  * Xperience by Kentico uses a different license key format.

 Additionally, object values or other content with **Macros** will not work correctly after the migration. Macro expressions are currently not supported for most data in Xperience by Kentico.

<!-- GETTING STARTED -->
## Get started

Follow the steps below to run the Kentico Migration Tool:

1. Clone or download the Migration.Toolkit source code from this repository.
2. Open the `Migration.Toolkit.sln` solution in Visual Studio.
3. Configure the options in the `Migration.Toolkit.CLI/appsettings.json` configuration file. See [`Migration.Toolkit.CLI/README.md - Configuration`](./Migration.Toolkit.CLI/README.md#Configuration) for details.
4. Rebuild the solution and restore all required NuGet packages.
5. Open the command line prompt.
6. Navigate to the output directory of the `Migration.Toolkit.CLI` project.
7. Run the `Migration.Toolkit.CLI.exe migrate` command.
    * The following example shows the command with all parameters for complete migration:

        ```powershell
        Migration.Toolkit.CLI.exe  migrate --sites --custom-modules --users --settings-keys --page-types --pages --attachments --contact-management --forms --media-libraries --data-protection --countries
        ```

8. Observe the command line output. The command output is also stored in a log file (`logs\log-<date>.txt` under the output directory by default), which you can review later.
9. Review the migration protocol, which provides information about the result of the migration, lists required manual steps, etc.

    * You can find the protocol in the location specified by the `MigrationProtocolPath` key in the `appsettings.json` configuration file.
    * For more information, see [`Migration.Toolkit.CLI/MIGRATION_PROTOCOL_REFERENCE.md`](./Migration.Toolkit.CLI/MIGRATION_PROTOCOL_REFERENCE.md).

The data is now migrated to the target Xperience by Kentico instance according to your configuration. See [`Migration.Toolkit.CLI/README.md`](./Migration.Toolkit.CLI/README.md) for detailed information about the migration CLI, configuration options, instructions related to individual object types, and manual migration steps.

## Changelog of recent updates

* **June 13, 2024**
  * Migration of categories to taxonomies is available
* **March 11, 2024**
  * Kentico Xperience 11 instances are supported as a source of migration
* **February 1, 2024**
  * Kentico Xperience 12 instances are supported as a source of migration

<!-- CONTRIBUTING -->
## Contributing

See [`CONTRIBUTING.md`](./CONTRIBUTING.md) to learn how to file issues, start discussions, and submit contributions.

Please provide all available information about the problem or error when submitting issues. If possible, include the command line output log file and migration protocol generated by the `Migration.Toolkit.CLI.exe migrate` command.

<!-- LICENSE -->
## License

Distributed under the MIT License. See [`LICENSE.md`](./LICENSE.md) for more information.

## Support

This contribution has **Full support by 7-day bug-fix policy**.

See [`SUPPORT.md`](https://github.com/Kentico/.github/blob/main/SUPPORT.md#full-support) for more information.

For any security issues see [`SECURITY.md`](https://github.com/Kentico/.github/blob/main/SECURITY.md).