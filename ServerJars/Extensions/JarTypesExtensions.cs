using ServerJarsAPI.Models;

namespace ServerJarsAPI.Extensions;

public static class JarTypesExtensions
{
    public static List<JarTypeItem> ToList(this JarTypes jarTypes)
    {
        List<JarTypeItem> list = new();

        list.AddRange(jarTypes.Vanilla.Select(v => new JarTypeItem() { Category = nameof(jarTypes.Vanilla).ToLower(), Type = v }));
        list.AddRange(jarTypes.Bedrock.Select(v => new JarTypeItem() { Category = nameof(jarTypes.Bedrock).ToLower(), Type = v }));
        list.AddRange(jarTypes.Servers.Select(v => new JarTypeItem() { Category = nameof(jarTypes.Servers).ToLower(), Type = v }));
        list.AddRange(jarTypes.Modded.Select(v => new JarTypeItem() { Category = nameof(jarTypes.Modded).ToLower(), Type = v }));
        list.AddRange(jarTypes.Proxies.Select(v => new JarTypeItem() { Category = nameof(jarTypes.Proxies).ToLower(), Type = v }));

        return list;
    }
}
