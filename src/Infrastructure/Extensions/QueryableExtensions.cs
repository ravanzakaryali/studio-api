using System.Collections.Generic;
namespace Space.Infrastructure.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TEntity> PredicateEntityActive<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, bool>> predicate)
        where TEntity : class, IBaseEntity
    {

        ExpressionHelper expressionHelper = new("IsDeleted", "IsActive");
        if (expressionHelper.ContainsMembers(predicate).Any(a=>a.Value == true))
        {
            query = query.IgnoreQueryFilters();
        }
        return query;
    }
    public static IQueryable<TEntity> GetIncludeQuery<TEntity>(this IQueryable<TEntity> query, params string[] includes) where TEntity : class, IBaseEntity
    {
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query;
        }
        return query;
    }
}
