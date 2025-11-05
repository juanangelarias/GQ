using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace GQ.Database.Extensions;

public static class QueryableExtensions
{
     public static IQueryable<TEntity>? OrderBy<TEntity>(this IQueryable<TEntity> source, string propertyName, bool desc)
    {
        var method = desc
            ? "OrderByDescending"
            : "OrderBy";

        var type = typeof(TEntity);
        var property = type.GetProperty(propertyName);
        var parameter = Expression.Parameter(type, "p");

        if (property == null)
            return null;

        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        var orderByExpression = Expression.Lambda(propertyAccess, parameter);
        var resultExpression = Expression.Call(
            typeof(Queryable), 
            method,
            [type, property.PropertyType], 
            source.Expression, 
            Expression.Quote(orderByExpression));

        return source.Provider.CreateQuery<TEntity>(resultExpression);
    }

    public static IQueryable<TEntity> Filter<TEntity>(this IQueryable<TEntity> source, string filter)
    {
        var method = typeof(Queryable)
            .GetMethods()
            .First(f =>
            {
                var parameters = f.GetParameters().ToList();
                return f is { Name: "Where", IsGenericMethodDefinition: true } && parameters.Count == 2;
            });

        var type = typeof(TEntity);

        var propNameConversionDictionary = new Dictionary<string, string>(
            typeof(TEntity)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(s => new KeyValuePair<string, string>(s.Name.ToLower(), s.Name)));
        var parameterValueMap = DetermineProperty(filter, propNameConversionDictionary);
        var propertyTypesDictionary = parameterValueMap
            .Select(s => type.GetProperty(s.Key))
            .ToList();
        
        // Parameter for the lambda (this is the entity that is being queried, not the search parameters)
        var parameter = Expression.Parameter(typeof(TEntity));
        
        // Property expressions for 'Where' predicate ... taken from filter values provided
        // Generate equal expressions for search query parameters
        var localBinaryExpressions = propertyTypesDictionary
            .Select(prop => Expression.Equal(
                Expression.MakeMemberAccess(parameter, prop!), 
                Expression.Constant(parameterValueMap[prop!.Name])))
            .ToList();
        
        // This still needs work
        var andAlso = Expression.AndAlso(localBinaryExpressions.First(), localBinaryExpressions.Last());
        var lambda = Expression.Lambda(andAlso, parameter);

        var genericMethod = method.MakeGenericMethod(typeof(TEntity));

        return (IQueryable<TEntity>)genericMethod.Invoke(genericMethod, [source, lambda])!;
    }

    private static Dictionary<string, string> DetermineProperty(string filter, Dictionary<string, string> dictionary)
    {
        var regex = new Regex(@"\w*:\s*.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        var matches = regex.Matches(filter);
        var searchCriteriaDictionary = new Dictionary<string, string>();
        foreach (Match match in matches)
        {
            var items = match.Value.Split(':');
            if (items.Length == 2)
            {
                searchCriteriaDictionary.Add(dictionary[items[0].ToLower()], items[1].Trim());
            }
        }

        return searchCriteriaDictionary;
    }
}