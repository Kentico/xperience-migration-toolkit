using System.Collections.Concurrent;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Migration.Toolkit.Common;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.Convertors;
using Migration.Toolkit.KX13.Context;
using Migration.Toolkit.KXO.Api;
using Migration.Toolkit.KXO.Context;
using Migration.Toolkit.KXO.Models;

namespace Migration.Toolkit.Core.Handlers;

public class MigrateAttachmentsCommandHandler: IRequestHandler<MigrateAttachmentsCommand, CommandResult>, IDisposable
{
    private readonly ILogger<MigrateAttachmentsCommandHandler> _logger;
    private readonly IDbContextFactory<KX13Context> _kx13ContextFactory;
    private readonly ToolkitConfiguration _toolkitConfiguration;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;
    private readonly KxoMediaFileFacade _mediaFileFacade;
    private readonly IDbContextFactory<KxoContext> _kxoContextFactory;
    private readonly AttachmentConvertor _attachmentConvertor;
    private readonly KxoContext _kxoDbContext;

    public MigrateAttachmentsCommandHandler(
            ILogger<MigrateAttachmentsCommandHandler> logger,
            IDbContextFactory<KX13.Context.KX13Context> kx13ContextFactory,
            ToolkitConfiguration toolkitConfiguration,
            PrimaryKeyMappingContext primaryKeyMappingContext,
            KxoMediaFileFacade mediaFileFacade,
            IDbContextFactory<KXO.Context.KxoContext> kxoContextFactory,
            AttachmentConvertor attachmentConvertor
    )
    {
        _logger = logger;
        _kx13ContextFactory = kx13ContextFactory;
        _toolkitConfiguration = toolkitConfiguration;
        _primaryKeyMappingContext = primaryKeyMappingContext;
        _mediaFileFacade = mediaFileFacade;
        _kxoContextFactory = kxoContextFactory;
        _attachmentConvertor = attachmentConvertor;

        _kxoDbContext = _kxoContextFactory.CreateDbContext();
    }
    
    public async Task<CommandResult> Handle(MigrateAttachmentsCommand request, CancellationToken cancellationToken)
    {
        var explicitSiteIdMapping = _toolkitConfiguration.RequireSiteIdExplicitMapping<KX13.Models.CmsSite>(s => s.SiteId).Keys.ToList();
        
        var kx13Context = _kx13ContextFactory.CreateDbContext();

        var kx13CmsAttachments = kx13Context.CmsAttachments.Where(x => explicitSiteIdMapping.Contains(x.AttachmentSiteId));
        
        var libraryNameMask = _toolkitConfiguration.TargetAttachmentMediaLibraryName;
        var targetMediaLibraryForSite = new ConcurrentDictionary<(string targetLibraryName, int targetSiteId), KXOM.MediaLibrary>();
        var targetSites = new ConcurrentDictionary<int, KXOM.CmsSite>();
        foreach (var kx13CmsAttachment in kx13CmsAttachments)
        {
            // TODO tk: 2022-06-29 kontrola
            // TODO tk: 2022-06-29 check if target media library exists for site

            // TODO tk: 2022-06-29 maybe not require and just log not found?
            var targetSiteId = _primaryKeyMappingContext.RequireMapFromSource<KX13.Models.CmsSite>(s => s.SiteId, kx13CmsAttachment.AttachmentSiteId);
            var targetSite = targetSites.GetOrAdd(targetSiteId, i => _kxoDbContext.CmsSites.Single(s => s.SiteId == targetSiteId));


            var targetLibraryName = libraryNameMask
                .Replace("{sitename}", targetSite.SiteName, StringComparison.InvariantCultureIgnoreCase)
                .Replace("{siteid}", targetSite.SiteId.ToString(), StringComparison.InvariantCultureIgnoreCase)
                ;
            
            var targetMediaLibrary = targetMediaLibraryForSite.GetOrAdd((targetLibraryName, targetSite.SiteId), s =>
            {
                var tml = _kxoDbContext.MediaLibraries.SingleOrDefault(ml => ml.LibrarySiteId == s.targetSiteId && ml.LibraryName == s.targetLibraryName);
                if (tml != null) return tml;
                
                tml = new MediaLibrary
                {
                    LibraryDisplayName = targetLibraryName,
                    LibraryName = targetLibraryName,
                    LibraryDescription = "",
                    LibraryFolder = targetLibraryName
                };
                _kxoDbContext.MediaLibraries.Add(tml);
                _kxoDbContext.SaveChanges();
                // TODO tk: 2022-06-29 handle db error
                
                return tml;
            });
            
            var mapped = _attachmentConvertor.ToMediaFile(kx13CmsAttachment, targetMediaLibrary.LibraryId);
            // TODO tk: 2022-06-29 report mapping - _migrationProtocol.MappedTarget(mapped);
            
            _mediaFileFacade.InsertMediaFile(mapped);
            // TODO tk: 2022-06-29 report result
        }

        return new GenericCommandResult();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}