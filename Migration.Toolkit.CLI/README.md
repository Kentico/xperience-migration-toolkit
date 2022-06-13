## CLI Usage

## Command migrate

> warning: only empty sites are currently fully supported.

Command usage `migrate --sites --users`

| Parameter                      | Description                             | Required parameters                      | Dependencies                         |
|--------------------------------|-----------------------------------------|------------------------------------------|--------------------------------------|
| `--sites`                      | Performs migration of Site objects      | Configuration of explicit SiteID mapping | none                                 |
| `--users`                      | Performs migration of User objects      | none                                     | `--sites`                            |
| `--contact-groups`             | Performs migration of User objects      | none                                     | none                                 |
| `--contact-management`         | Performs migration of Contact groups    | none                                     | `--users`                            |
| `--data-protection`            |                                         |                                          | `--sites`, `--users`                 |
| `--forms`                      |                                         |                                          | `--sites`                            |
| `--media-libraries`            |                                         |                                          | `--sites`, `--users`                 |
| `--page-types`                 |                                         |                                          | `--sites`                            |
| `--pages`                      |                                         | `--culture`                              | `--sites`, `--users`, `--page-types` |
| `--settings-keys`              |                                         |                                          | `--sites`                            |
| `--culture <culture>`          |                                         |                                          |                                      |
| `----bypass-dependency-check`  | Tool will skip command dependency check |                                          |                                      |

### Examples

1) `Migration.Toolkit.CLI.exe migrate --sites --users --settings-keys --media-libraries --page-types --pages --culture en-US`
3) `Migration.Toolkit.CLI.exe migrate --page-types --pages --culture en-US --bypass-dependency-check` - if you want to retry pages migration when `--sites` and `--users` migration was already performed

## Configuration

To run tool, configure `appsettings.json` file:

```json
{
  "SourceConnectionString": "[TODO ConnectionString]",
  "TargetConnectionString": "[TODO ConnectionString]",
  "TargetKxoApiSettings": {
    "ConnectionStrings": {
      "CMSConnectionString": "[TODO ConnectionString]"
    }
  },
  "EntityConfigurations": {
    "CMS_Site": {
      "ExplicitPrimaryKeyMapping": {
        "SiteID": {
          "1": 1
        }
      }
    }
  }
}
```

| Property path                                                  | Description                                                                                     |
|----------------------------------------------------------------|-------------------------------------------------------------------------------------------------|
| SourceConnectionString                                         | Source kentico instance connection string for tool usage                                        |
| TargetConnectionString                                         | Target (KXO) instance connection string for tool usage                                          |
| TargetKxoApiSettings                                           | KXO Api Settings -  `ConnectionStrings.CMSConnectionString` is required                         |
| EntityConfigurations.CMS_Site.ExplicitPrimaryKeyMapping.SiteID | Required - mapping of source siteId to target siteId (currentyl site creation is not supported) |

