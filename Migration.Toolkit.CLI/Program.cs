using System.Globalization;
using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Migration.Toolkit.Common;
using Migration.Toolkit.Core;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.Services;
using Migration.Toolkit.KX13;
using Migration.Toolkit.KXO;
using Migration.Toolkit.KXO.Api;
using Migration.Toolkit.KXO.Context;

// https://docs.microsoft.com/en-us/dotnet/core/extensions/configuration

var config = new ConfigurationBuilder()
        .SetBasePath(Environment.CurrentDirectory)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
        .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: false)
        // .AddEnvironmentVariables()
        .Build()
    ;

var settings = config.GetRequiredSection("Settings").Get<ToolkitConfiguration>();

var services = new ServiceCollection();

services
    .AddLogging(builder =>
    {
        builder.AddConfiguration(config.GetSection("Logging"));
        builder.AddSimpleConsole(options =>
        {
            options.IncludeScopes = true;
            options.SingleLine = true;
            options.TimestampFormat = "hh:mm:ss.fff ";
        });
        builder.AddFile(config.GetSection("Logging"));
    });

services.UseKx13DbContext(settings);
services.UseKxoDbContext(settings);
services.UseKxoApi(config.GetRequiredSection("Settings").GetRequiredSection("TargetKxoApiSettings"), settings.TargetCmsDirPath);
services.AddSingleton(settings);
services.UseToolkitCore();

await using var serviceProvider = services.BuildServiceProvider();
using var scope = serviceProvider.CreateScope();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

string Yellow(string ctext) => $"\x1b[33m{ctext}\x1b[0m";
string Green(string ctext) => $"\x1b[32m{ctext}\x1b[0m";
string Red(string ctext) => $"\x1b[31m{ctext}\x1b[0m";

void WriteCommandDesc(string desc, string commandMoniker)
{
    Console.WriteLine($"{Yellow(commandMoniker)}: {desc}");
}

bool RequireParameter(string paramName, out string paramValue)
{
    if (Array.IndexOf(args, paramName) is int cIdx and > -1 && args.Length > cIdx + 1)
    {
        paramValue = args[cIdx + 1];
        return true;
    }

    Console.WriteLine(Red($"Parameter {paramName} is reqiured."));
    paramValue = null;
    return false;
}

bool RequireNumberParameter(string paramName, out int? paramValue)
{
    if (Array.IndexOf(args, paramName) is int cIdx and > -1 && args.Length > cIdx + 1)
    {
        if(int.TryParse(args[cIdx + 1], out var value))
        {
            paramValue = value;
            return true;
        }
        else
        {
            Console.WriteLine(Red($"Parameter {paramName} is not int."));
            paramValue = null;
            return false;
        }
    }

    Console.WriteLine(Red($"Parameter {paramName} is reqiured."));
    paramValue = null;
    return false;
}


var mappingContext = scope.ServiceProvider.GetRequiredService<PrimaryKeyMappingContext>();
var toolkitConfiguration = scope.ServiceProvider.GetRequiredService<ToolkitConfiguration>();
var tableTypeLookupService = scope.ServiceProvider.GetRequiredService<TableReflectionService>();
foreach (var (k, ek) in toolkitConfiguration.EntityConfigurations)
{
    var tableType = tableTypeLookupService.GetSourceTableTypeByTableName(k);
    
    foreach (var (kPkName, mappings) in ek.ExplicitPrimaryKeyMapping)
    {
        foreach (var (kPk, vPk) in mappings)
        {
            // TODO tk: 2022-05-26 report incorrect property setting
            if (int.TryParse(kPk, out var kPkParsed) && vPk.HasValue)
            {
                mappingContext.SetMapping(tableType, kPkName, kPkParsed, vPk.Value);
            }    
        }
    }    
}


void PrintCommandDescriptions()
{
    WriteCommandDesc($"starts migration of {Green(MigratePageTypesCommand.MonikerFriendly)}", $"migrate --{MigratePageTypesCommand.Moniker}");
    WriteCommandDesc($"starts migration of {Green(MigratePagesCommand.MonikerFriendly)}", $"migrate --{MigratePagesCommand.Moniker}");
    WriteCommandDesc($"starts migration of {Green(MigrateSettingKeysCommand.MonikerFriendly)}", $"migrate --{MigrateSettingKeysCommand.Moniker}");
    WriteCommandDesc($"starts migration of {Green(MigrateContactGroupsCommand.MonikerFriendly)}", $"migrate --{MigrateContactGroupsCommand.Moniker}");
    WriteCommandDesc($"starts migration of {Green(MigrateContactManagementCommand.MonikerFriendly)}", $"migrate --{MigrateContactManagementCommand.Moniker}");
    WriteCommandDesc($"starts migration of {Green(MigrateDataProtectionCommand.MonikerFriendly)}", $"migrate --{MigrateDataProtectionCommand.Moniker}");
    WriteCommandDesc($"starts migration of {Green(MigrateFormsCommand.MonikerFriendly)}", $"migrate --{MigrateFormsCommand.Moniker}");
    WriteCommandDesc($"starts migration of {Green(MigrateMediaLibrariesCommand.MonikerFriendly)}", $"migrate --{MigrateMediaLibrariesCommand.Moniker}");
    WriteCommandDesc($"starts migration of {Green(MigrateSitesCommand.MonikerFriendly)}", $"migrate --{MigrateSitesCommand.Moniker}");
    WriteCommandDesc($"starts migration of {Green(MigrateUsersCommand.MonikerFriendly)}", $"migrate --{MigrateUsersCommand.Moniker}");
    WriteCommandDesc($"starts migration of {Green(MigrateAttachmentsCommand.MonikerFriendly)}", $"migrate --{MigrateAttachmentsCommand.Moniker}");
    
    // WriteCommandDesc($"starts migration of {Green(MigrateWebFarmsCommand.MonikerFriendly)}", $"migrate --{MigrateWebFarmsCommand.Moniker}");
    // Console.WriteLine($"Run with option {Yellow("--dry")} to execute command without persistence");
}

var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();
var kxoContext = scope.ServiceProvider.GetRequiredService<IDbContextFactory<KxoContext>>().CreateDbContext();

var argsQ = new Queue<string>(args);
var commands = new List<ICommand>();
bool firstHaveToBeMigrate = true;
var bypassDependencyCheck = false;
while (argsQ.TryDequeue(out var arg))
{
    var cultureCode = "";
    
    // TODO tk: 2022-06-23 ! konfigurovat site přes SiteName (ponechat aktuální přístup)
    if (RequireNumberParameter("--siteId", out var siteId) && siteId is int sid && kxoContext.CmsSites.FirstOrDefault()?.SiteId is int targetSiteId)
    {
        toolkitConfiguration.AddExplicitMapping<Migration.Toolkit.KX13.Models.CmsSite>(s => s.SiteId, sid, targetSiteId);
        // mappingContext.SetMapping<Migration.Toolkit.KX13.Models.CmsSite>(s => s.SiteId, sid, targetSiteId);
    }
    else
    {
        return;
    }

    if (arg.IsIn("help", "h"))
    {
        PrintCommandDescriptions();
        break;
    }

    if (arg == "migrate" && firstHaveToBeMigrate)
    {
        firstHaveToBeMigrate = false;
        continue;
    }

    if (arg == "--culture")
    {
        continue;
    }
    
    if (arg == "--site")
    {
        continue;
    }
    
    if (arg == "--bypass-dependency-check")
    {
        bypassDependencyCheck = true;
        continue;
    }

    if (firstHaveToBeMigrate)
    {
        Console.WriteLine($"First must be command, for example {Green("migrate")}");
        PrintCommandDescriptions();
        break;
    }

    if (arg == $"--{MigrateContactGroupsCommand.Moniker}")
    {
        commands.Add(new MigrateContactGroupsCommand());
        continue;
    }

    if (arg == $"--{MigrateContactManagementCommand.Moniker}")
    {
        commands.Add(new MigrateContactManagementCommand());
        continue;
    }

    if (arg == $"--{MigrateDataProtectionCommand.Moniker}")
    {
        RequireNumberParameter("--batchSize", out var batchSize);
        commands.Add(new MigrateDataProtectionCommand(batchSize));
        continue;
    }

    if (arg == $"--{MigrateFormsCommand.Moniker}")
    {
        commands.Add(new MigrateFormsCommand());
        continue;
    }

    if (arg == $"--{MigrateMediaLibrariesCommand.Moniker}")
    {
        commands.Add(new MigrateMediaLibrariesCommand());
        continue;
    }

    if (arg == $"--{MigratePageTypesCommand.Moniker}")
    {
        commands.Add(new MigratePageTypesCommand());
        continue;
    }

    if (arg == $"--{MigratePagesCommand.Moniker}")
    {
        if (RequireParameter("--culture", out var culture))
        {
            try
            {
                if (CultureInfo.GetCultureInfo(culture) is CultureInfo cultureInfo) // TODO tk: 2022-05-18 also check in kentico db for validity
                {
                    cultureCode = cultureInfo.Name;
                }
            }
            catch (CultureNotFoundException cnfex)
            {
                Console.WriteLine($"{Red($"Culture '{culture}' not found!")}");
                break;
            }

            commands.Add(new MigratePagesCommand(cultureCode));
            continue;
        }
    }

    if (arg == $"--{MigrateSettingKeysCommand.Moniker}")
    {
        commands.Add(new MigrateSettingKeysCommand());
        continue;
    }

    if (arg == $"--{MigrateSitesCommand.Moniker}")
    {
        commands.Add(new MigrateSitesCommand());
        continue;
    }

    if (arg == $"--{MigrateUsersCommand.Moniker}")
    {
        commands.Add(new MigrateUsersCommand());
        continue;
    }
    
    if (arg == $"--{MigrateAttachmentsCommand.Moniker}")
    {
        if (RequireParameter("--culture", out var culture))
        {
            try
            {
                if (CultureInfo.GetCultureInfo(culture) is CultureInfo cultureInfo) // TODO tk: 2022-05-18 also check in kentico db for validity
                {
                    cultureCode = cultureInfo.Name;
                }
            }
            catch (CultureNotFoundException cnfex)
            {
                Console.WriteLine($"{Red($"Culture '{culture}' not found!")}");
                break;
            }

            commands.Add(new MigrateAttachmentsCommand(cultureCode));
            continue;
        }
    }

    // if (arg == $"--{MigrateWebFarmsCommand.Moniker}")
    // {
    //     commands.Add(new MigrateWebFarmsCommand());
    //     continue;
    // }

    // Console.WriteLine($"Invalid arguments, for help call with command {Yellow("help")}, usable commands:");
    // PrintCommandDescriptions();
}

kxoContext.Dispose();

var satisfiedDependencies = new HashSet<Type>();
var dependenciesSatisfied = true;
if (!bypassDependencyCheck)
{
    foreach (var command in commands)
    {
        var commandType = command.GetType();
        satisfiedDependencies.Add(commandType);
        foreach (var commandDependency in command.Dependencies)
        {
            if (!satisfiedDependencies.Contains(commandDependency))
            {
                var cmdMoniker = commandType.GetProperty("Moniker", BindingFlags.Static | BindingFlags.Public)?.GetValue(null);
                var cmdMonikerNeeded = commandDependency.GetProperty("Moniker", BindingFlags.Static | BindingFlags.Public)?.GetValue(null);

                dependenciesSatisfied = false;

                Console.WriteLine($"Migration {Yellow($"--{cmdMoniker}")} needs to run migration {Red($"--{cmdMonikerNeeded}")} before.");
            }
        }
    }
}

if (!dependenciesSatisfied)
{
    return;
}

foreach (var command in commands)
{
    await mediatr.Send(command);
    Console.WriteLine($"Command {command.GetType().Name} is completed");
}

Console.WriteLine("Press any key to exit...");
Console.ReadKey();