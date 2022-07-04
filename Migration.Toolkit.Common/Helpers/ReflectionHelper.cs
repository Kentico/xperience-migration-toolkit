using System.Diagnostics;
using System.Reflection;

namespace Migration.Toolkit.Common.Helpers;

public class ReflectionHelper<T>
{
    public static Type CurrentType { get; set; }
    
    static ReflectionHelper()
    {
        CurrentType = typeof(T);
        
        var i = 0;
        PropertyGetterMaps = new Dictionary<string, ObjectPropertyGetterMap>();
        foreach (var propertyInfo in CurrentType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
        {
            if (PropertyGetterMaps.ContainsKey(propertyInfo.Name))
            {
                Debugger.Break();
            }
            
            PropertyGetterMaps.Add(propertyInfo.Name, new ObjectPropertyGetterMap
            {
                PropertyName = propertyInfo.Name,
                PropertyGetMethod = propertyInfo.GetMethod,
                PropertyIndex = i++,
            });
        }
        
        StaticPropertyGetterMaps = new Dictionary<string, ObjectPropertyGetterMap>();
        foreach (var propertyInfo in CurrentType.GetProperties(BindingFlags.Public | BindingFlags.Static))
        {
            if (StaticPropertyGetterMaps.ContainsKey(propertyInfo.Name))
            {
                Debugger.Break();
            }
            
            StaticPropertyGetterMaps.Add(propertyInfo.Name, new ObjectPropertyGetterMap
            {
                PropertyName = propertyInfo.Name,
                PropertyGetMethod = propertyInfo.GetMethod,
                PropertyIndex = i++,
            });
        }
    }

    public static Dictionary<string, ObjectPropertyGetterMap> StaticPropertyGetterMaps { get; private set; }
    public static Dictionary<string, ObjectPropertyGetterMap> PropertyGetterMaps { get; private set; }

    public static List<TAttribute> GetAttributes<TAttribute>() where TAttribute: Attribute => Attribute
        .GetCustomAttributes(typeof(T)).Aggregate(new List<TAttribute>(), (list, attribute) =>
        {
            if (attribute is TAttribute targetAttribute)
            {
                list.Add(targetAttribute);
            }

            return list;
        });

    public static TAttribute? GetFirstAttributeOrNull<TAttribute>() where TAttribute : Attribute => GetAttributes<TAttribute>().FirstOrDefault();

    public static IEnumerable<ObjectPropertyGetterMap> GetPropertyGetterMaps() => PropertyGetterMaps.Values;

    public static object? GetPropertyValue(T obj, string propertyName)
    {
        if (!PropertyGetterMaps.ContainsKey(propertyName))
        {
            throw new InvalidOperationException($"Property '{propertyName}' doesn't exist on object of type '{CurrentType.FullName}'");
        }

        if (obj == null)
        {
            throw new NullReferenceException($"Null reference, ReflectionHelper.GetPropertyValue needs obj argument reference");
        }
        
        return PropertyGetterMaps[propertyName].PropertyGetMethod!.Invoke(obj, Array.Empty<object>());
    }

    public static object? GetStaticPropertyValue(string propertyName)
    {
        if (!StaticPropertyGetterMaps.ContainsKey(propertyName))
        {
            throw new InvalidOperationException($"Property '{propertyName}' doesn't exist on object of type '{CurrentType.FullName}'");
        }

        return StaticPropertyGetterMaps[propertyName].PropertyGetMethod!.Invoke(null, Array.Empty<object>());
    }
}

public class ObjectPropertyGetterMap
{
    public string PropertyName { get; set; }
    public MethodInfo? PropertyGetMethod { get; set; }
    public int PropertyIndex { get; set; }
}

public class ReflectionHelper
{
    public Type CurrentType { get; }
    
    public ReflectionHelper(Type type)
    {
        CurrentType = type;
    }

    public List<TAttribute> GetAttributes<TAttribute>() where TAttribute: Attribute => Attribute
        .GetCustomAttributes(CurrentType).Aggregate(new List<TAttribute>(), (list, attribute) =>
        {
            if (attribute is TAttribute targetAttribute)
            {
                list.Add(targetAttribute);
            }

            return list;
        });

    public TAttribute? GetFirstAttributeOrNull<TAttribute>() where TAttribute : Attribute => GetAttributes<TAttribute>().FirstOrDefault();
}