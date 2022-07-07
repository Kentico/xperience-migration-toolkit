using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.Core.Helpers;

public static class EfCoreHelper
{
    // public static Task EnableIdentityInsert<T>(this DbContext context) => SetIdentityInsert<T>(context, enable: true);
    // public static Task DisableIdentityInsert<T>(this DbContext context) => SetIdentityInsert<T>(context, enable: false);
    //
    // private static Task SetIdentityInsert<T>(DbContext context, bool enable)
    // {
    //     var entityType = context.Model.FindEntityType(typeof(T));
    //     var value = enable ? "ON" : "OFF";
    //     return context.Database.ExecuteSqlRawAsync(
    //         $"SET IDENTITY_INSERT {entityType.GetSchema()}.{entityType.GetTableName()} {value}");
    // }
    //
    // public static void SaveChangesWithIdentityInsert<T>(this DbContext context)
    // {
    //     using var transaction = context.Database.BeginTransaction();
    //     context.EnableIdentityInsert<T>();
    //     context.SaveChanges();
    //     context.DisableIdentityInsert<T>();
    //     transaction.Commit();
    // }

    public static T Clone<T>(DbContext dbContext, T entity) where T: new()
    {
        var clone = new T();
        var values = dbContext.Entry(entity).CurrentValues.Clone();
        dbContext.Entry(clone).CurrentValues.SetValues(values);
        return clone;
    }
}