using System.Reflection;
using AutoMapper;

namespace Application.Common.Mappings;

public static class MappingExtensions
{
    public static void ApplyMappingsFromAssembly(this Profile profile, Assembly assembly)
    {
        var types = assembly.GetTypes()
            .Where(t => t != typeof(IMapFrom<>) && t != typeof(IMapTo<>)
                                                && t.GetInterfaces().Contains(typeof(IMapped)))
            .ToList();

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);
            var methodInfo = type.GetMethod(nameof(IMapped.Mapping))
                             ?? type.GetInterface(nameof(IMapped))?.GetMethod(nameof(IMapped.Mapping));

            methodInfo?.Invoke(instance, new object[] { profile });
        }
    }
}