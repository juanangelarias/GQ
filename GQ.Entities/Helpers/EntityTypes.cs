using System.Reflection;

namespace GQ.Entities.Helpers;

public class EntityTypes
{
    public static IEnumerable<Type> GetEntityTypes<T>()
    {
        return Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where( r => typeof( T ).IsAssignableFrom( r ) && !r.IsInterface );
    }

    public static Type[] GetTypesInNamespace( Assembly assembly, string nameSpace )
    {
        return assembly
            .GetTypes()
            .Where( r => string.Equals( r.Namespace, nameSpace, StringComparison.Ordinal ) )
            .ToArray();
    }
}