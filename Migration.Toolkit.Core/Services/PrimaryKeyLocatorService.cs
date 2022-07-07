using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Migration.Toolkit.Common;

namespace Migration.Toolkit.Core.Services;

public class PrimaryKeyLocatorService : IPrimaryKeyLocatorService
{
    private readonly ILogger<PrimaryKeyLocatorService> _logger;
    private readonly IDbContextFactory<KXO.Context.KxoContext> _kxoContextFactory;
    private readonly IDbContextFactory<KX13.Context.KX13Context> _kx13ContextFactory;

    public PrimaryKeyLocatorService(
        ILogger<PrimaryKeyLocatorService> logger,
        IDbContextFactory<KXO.Context.KxoContext> kxoContextFactory,
        IDbContextFactory<KX13.Context.KX13Context> kx13ContextFactory
    )
    {
        _logger = logger;
        _kxoContextFactory = kxoContextFactory;
        _kx13ContextFactory = kx13ContextFactory;
    }

    private class KeyEqualityComparerWithLambda<T> : IEqualityComparer<T>
    {
        private readonly Func<T?, T?, bool> _equalityComparer;

        public KeyEqualityComparerWithLambda(Func<T?,T?,bool> equalityComparer)
        {
            _equalityComparer = equalityComparer;
        }

        public bool Equals(T? x, T? y) => _equalityComparer.Invoke(x, y);

        public int GetHashCode(T obj) => obj?.GetHashCode() ?? 0;
    }

    private record CmsUserKey(Guid UserGuid, string UserName);

    public IEnumerable<SourceTargetKeyMapping> SelectAll<T>(Expression<Func<T, object>> keyNameSelector)
    {
        using var kxoContext = _kxoContextFactory.CreateDbContext();
        using var kx13Context = _kx13ContextFactory.CreateDbContext();
        
        var sourceType = typeof(T);
        var memberName = keyNameSelector.GetMemberName();

        _logger.LogTrace("Preload of entity {entity} member {memberName} mapping requested", sourceType.Name, memberName);
        
        if (sourceType == typeof(KX13.Models.CmsUser) && memberName == nameof(KX13M.CmsUser.UserId))
        {
            var sourceUsers = kx13Context.CmsUsers.Select(x => new { x.UserId, x.UserGuid, x.UserName }).ToList();
            var targetUsers = kxoContext.CmsUsers.Select(x => new { x.UserId, x.UserName, x.UserGuid }).ToList();

            var result = sourceUsers.Join(targetUsers,
                a => new CmsUserKey(a.UserGuid, a.UserName),
                b => new CmsUserKey(b.UserGuid, b.UserName),
                (a, b) => new SourceTargetKeyMapping(a.UserId, b.UserId),
                new KeyEqualityComparerWithLambda<CmsUserKey>((ak, bk) => (ak?.UserGuid == bk?.UserGuid || ak?.UserName == bk?.UserName) && ak != null && bk != null)
            );
            
            foreach (var resultingMapping in result)
            {
                yield return resultingMapping;
            }
            
            yield break;
        }

        if (sourceType == typeof(KX13.Models.OmContact) && memberName == nameof(KX13M.OmContact.ContactId))
        {
            var source = kx13Context.OmContacts
                .OrderBy(c => c.ContactCreated)
                .Select(x => new { x.ContactId, x.ContactGuid }).ToList();
            var target = kxoContext.OmContacts
                .OrderBy(c => c.ContactCreated)
                .Select(x => new { x.ContactId, x.ContactGuid }).ToList();

            var result = source.Join(target,
                a => a.ContactGuid,
                b => b.ContactGuid,
                (a, b) => new SourceTargetKeyMapping(a.ContactId, b.ContactId)
            );
            
            foreach (var resultingMapping in result)
            {
                yield return resultingMapping;
            }
            
            yield break;
        }

        if (sourceType == typeof(KX13.Models.CmsTree) && memberName == nameof(KX13M.CmsTree.NodeId))
        {
            var source = kx13Context.CmsTrees.Select(x => new { x.NodeId, x.NodeGuid }).ToList();
            var target = kxoContext.CmsTrees.Select(x => new { x.NodeId, x.NodeGuid }).ToList();

            var result = source.Join(target,
                a => a.NodeGuid,
                b => b.NodeGuid,
                (a, b) => new SourceTargetKeyMapping(a.NodeId, b.NodeId)
            );
            
            foreach (var resultingMapping in result)
            {
                yield return resultingMapping;
            }
            
            yield break;
        }
        
        if (sourceType == typeof(KX13.Models.CmsState) && memberName == nameof(KX13M.CmsState.StateId))
        {
            var source = kx13Context.CmsStates.Select(x => new { x.StateId, x.StateName }).ToList();
            var target = kxoContext.CmsStates.Select(x => new {  x.StateId, x.StateName }).ToList();

            var result = source.Join(target,
                a => a.StateName,
                b => b.StateName,
                (a, b) => new SourceTargetKeyMapping(a.StateId, b.StateId)
            );
            
            foreach (var resultingMapping in result)
            {
                yield return resultingMapping;
            }
            
            yield break;
        }
        
        if (sourceType == typeof(KX13.Models.CmsCountry) && memberName == nameof(KX13M.CmsCountry.CountryId))
        {
            var source = kx13Context.CmsCountries.Select(x => new { x.CountryId, x.CountryName }).ToList();
            var target = kxoContext.CmsCountries.Select(x => new {  x.CountryId, x.CountryName }).ToList();

            var result = source.Join(target,
                a => a.CountryName,
                b => b.CountryName,
                (a, b) => new SourceTargetKeyMapping(a.CountryId, b.CountryId)
            );
            
            foreach (var resultingMapping in result)
            {
                yield return resultingMapping;
            }
            
            yield break;
        }
        
            

        throw new NotImplementedException();
    }

    public bool TryLocate<T>(Expression<Func<T, object>> keyNameSelector, int sourceId, out int targetId)
    {
        using var kxoContext = _kxoContextFactory.CreateDbContext();
        using var kx13Context = _kx13ContextFactory.CreateDbContext();

        // var memberName = keyNameSelector.GetMemberName();
        // var entityType = kx13Context.Model.FindEntityType(typeof(T));

        // TODO tk: 2022-05-18 can be done smarter => deferred to optimizations 
        var sourceType = typeof(T);
        targetId = -1;
        try
        {
            if (sourceType == typeof(KX13.Models.CmsClass))
            {
                var kx13Guid = kx13Context.CmsClasses.Where(c => c.ClassId == sourceId).Select(x => x.ClassGuid).Single();
                targetId = kxoContext.CmsClasses.Where(x => x.ClassGuid == kx13Guid).Select(x => x.ClassId).Single();
                return true;
            }

            if (sourceType == typeof(KX13.Models.CmsUser))
            {
                var kx13User = kx13Context.CmsUsers.Where(c => c.UserId == sourceId).Select(x => new { x.UserGuid, x.UserName }).Single();
                targetId = kxoContext.CmsUsers.Where(x => x.UserGuid == kx13User.UserGuid || x.UserName == kx13User.UserName).Select(x => x.UserId)
                    .Single();
                return true;
            }

            if (sourceType == typeof(KX13.Models.CmsSite))
            {
                var kx13Guid = kx13Context.CmsSites.Where(c => c.SiteId == sourceId).Select(x => x.SiteGuid).Single();
                targetId = kxoContext.CmsSites.Where(x => x.SiteGuid == kx13Guid).Select(x => x.SiteId).Single();
                return true;
            }

            // target.ContactStateId = _primaryKeyMappingContext.MapFromSource<K13M.CmsState>(u => u.StateId, source.ContactStateId);
            if (sourceType == typeof(KX13.Models.CmsState))
            {
                var kx13Guid = kx13Context.CmsStates.Where(c => c.StateId == sourceId).Select(x => x.StateGuid).Single();
                targetId = kxoContext.CmsStates.Where(x => x.StateGuid == kx13Guid).Select(x => x.StateId).Single();
                return true;
            }

            // target.ContactCountryId = _primaryKeyMappingContext.MapFromSource<K13M.CmsCountry>(u => u.CountryId, source.ContactCountryId);
            if (sourceType == typeof(KX13.Models.CmsCountry))
            {
                var kx13Guid = kx13Context.CmsCountries.Where(c => c.CountryId == sourceId).Select(x => x.CountryGuid).Single();
                targetId = kxoContext.CmsCountries.Where(x => x.CountryGuid == kx13Guid).Select(x => x.CountryId).Single();
                return true;
            }

            // target.ContactStatusId = _primaryKeyMappingContext.MapFromSource<K13M.OmContactStatus>(u => u.ContactStatusId, source.ContactStatusId);
            if (sourceType == typeof(KX13.Models.OmContactStatus))
            {
                var kx13Guid = kx13Context.OmContactStatuses.Where(c => c.ContactStatusId == sourceId).Select(x => x.ContactStatusName).Single();
                targetId = kxoContext.OmContactStatuses.Where(x => x.ContactStatusName == kx13Guid).Select(x => x.ContactStatusId).Single();
                return true;
            }

            //target.ConsentAgreementContactId = _primaryKeyMappingContext.RequireMapFromSource<K13M.OmContact>(c => c.ContactId, source.ConsentAgreementContactId);
            if (sourceType == typeof(KX13.Models.OmContact))
            {
                // TODO tk: 2022-06-13 might be good to optimize
                var kx13Guid = kx13Context.OmContacts.Where(c => c.ContactId == sourceId).Select(x => x.ContactGuid).Single();
                targetId = kxoContext.OmContacts.Where(x => x.ContactGuid == kx13Guid).Select(x => x.ContactId).Single();
                return true;
            }

            if (sourceType == typeof(KX13.Models.CmsTree))
            {
                // carefull - cms.root will have different guid
                var kx13Guid = kx13Context.CmsTrees.Where(c => c.NodeId == sourceId).Select(x => x.NodeGuid).Single();
                targetId = kxoContext.CmsTrees.Where(x => x.NodeGuid == kx13Guid).Select(x => x.NodeId).Single();
                return true;
            }
        }
        catch (InvalidOperationException ioex)
            // when(ioex.Message.Contains("SequenceContainsN"))
        {
            _logger.LogWarning("Mapping {sourceFullType} primary key: {sourceId} failed, {message}", sourceType.FullName, sourceId, ioex.Message);
            return false;
        }
        finally
        {
            if (targetId != -1)
            {
                _logger.LogTrace("Mapping {sourceFullType} primary key: {sourceId} to {targetId}", sourceType.FullName, sourceId, targetId);
            }
        }

        _logger.LogError("Mapping {sourceFullType} primary key is not supported", sourceType.FullName);
        targetId = -1;
        return false;
    }
}