using MediatR;
using Migration.Toolkit.Core.Abstractions;

namespace Migration.Toolkit.Core;

public interface ICommand
{
    Type[] Dependencies { get; }
}

public record MigrateContactGroupsCommand(): IRequest<GenericCommandResult>, ICommand
{
    public static string Moniker => "contact-groups";
    public static string MonikerFriendly => "Contact groups";
    
    public Type[] Dependencies => new[] { typeof(MigrateUsersCommand), typeof(MigrateSitesCommand) };
}

public record MigrateContactManagementCommand(): IRequest<GenericCommandResult>, ICommand
{
    public static string Moniker => "contact-management";
    public static string MonikerFriendly => "Contact management";
    
    public Type[] Dependencies => new[] { typeof(MigrateSitesCommand) };
}

public record MigrateDataProtectionCommand(int? BatchSize) : IRequest<GenericCommandResult>, ICommand
{
    public static string Moniker => "data-protection";
    public static string MonikerFriendly => "Data protection";
    
    public Type[] Dependencies => new[] { typeof(MigrateSitesCommand) };
}

public record MigrateFormsCommand() : IRequest<GenericCommandResult>, ICommand
{
    public static string Moniker => "forms";
    public static string MonikerFriendly => "Forms";
    
    public Type[] Dependencies => new[]
    {
        typeof(MigrateSitesCommand)
        // TODO tk: 2022-05-26: require forms class first
    };
}

public record MigrateMediaLibrariesCommand(): IRequest<GenericCommandResult>, ICommand
{
    public static string Moniker => "media-libraries";
    public static string MonikerFriendly => "Media libraries";
    
    public Type[] Dependencies => new[] { typeof(MigrateSitesCommand), typeof(MigrateUsersCommand) };
}

public record MigratePageTypesCommand(): IRequest<MigratePageTypesResult>, ICommand
{
    public static string Moniker => "page-types";
    public static string MonikerFriendly => "Page types";
    
    public Type[] Dependencies => new[] { typeof(MigrateSitesCommand) };
}

public record MigratePagesCommand(string CultureCode): IRequest<CommandResult>, ICommand
{
    public static string Moniker => "pages";
    public static string MonikerFriendly => "Pages";
    
    public Type[] Dependencies => new[] { typeof(MigrateSitesCommand), typeof(MigrateUsersCommand), typeof(MigratePageTypesCommand) };
}

public record MigrateSettingKeysCommand() : IRequest<MigrateSettingsKeysResult>, ICommand
{
    public static string Moniker => "settings-keys";
    public static string MonikerFriendly => "Settings keys";
    
    public Type[] Dependencies => new[] { typeof(MigrateSitesCommand) };
}

public record MigrateSitesCommand(): IRequest<GenericCommandResult>, ICommand
{
    public static string Moniker => "sites";
    public static string MonikerFriendly => "Sites";
    
    public Type[] Dependencies => new Type[] { };
}

public record MigrateUsersCommand(): IRequest<GenericCommandResult>, ICommand
{
    public static string Moniker => "users";
    public static string MonikerFriendly => "Users";
    
    public Type[] Dependencies => new[] { typeof(MigrateSitesCommand) };
}

// public record MigrateWebFarmsCommand(): IRequest<GenericCommandResult>, ICommand
// {
//     public static string Moniker => "web-farms";
//     public static string MonikerFriendly => "Web farms";
//     
//     public Type[] Dependencies => new[] { typeof(MigrateSitesCommand) };
// }