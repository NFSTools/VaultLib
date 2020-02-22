using VaultLib.Core;

namespace VaultLib.Frameworks.Speed
{
    /// <summary>
    /// Helper class for libraries that use VaultLib.Frameworks.Speed
    /// </summary>
    public static class SpeedFramework
    {
        /// <summary>
        /// Registers the framework types with the given game IDs
        /// </summary>
        /// <param name="games">The game IDs to register the framework types with</param>
        public static void Register(params string[] games)
        {
            TypeRegistry.RegisterAssemblyTypes(typeof(SpeedFramework).Assembly, games);
        }
    }
}