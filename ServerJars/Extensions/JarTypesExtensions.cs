using ServerJarsAPI.Models;

namespace ServerJarsAPI.Extensions;

public static class JarTypesExtensions
{
    public static Dictionary<string, IEnumerable<string>> AsDictionary(this JarTypes jarTypes)
    {
        return new()
        {
            { nameof(jarTypes.Vanilla).ToLower(), jarTypes.Vanilla },
            { nameof(jarTypes.Bedrock).ToLower(), jarTypes.Bedrock },
            { nameof(jarTypes.Servers).ToLower(), jarTypes.Servers },
            { nameof(jarTypes.Modded).ToLower(), jarTypes.Modded },
            { nameof(jarTypes.Proxies).ToLower(), jarTypes.Proxies },
        };
    }
}
