using VaultLib.Core;

namespace VaultLib.Frameworks.Speed;

/// <summary>
/// Helper class for libraries that use VaultLib.Frameworks.Speed
/// </summary>
public static class SpeedFramework
{
    /// <summary>
    /// Registers the framework types.
    /// </summary>
    /// <param name="registry">The type registry to register the types with</param>
    public static void Register<TKey>(TypeRegistry<TKey> registry)
    {
        registry.RegisterAssemblyTypes(typeof(SpeedFramework).Assembly);
    }
}