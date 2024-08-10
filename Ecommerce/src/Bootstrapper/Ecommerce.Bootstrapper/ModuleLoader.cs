using Ecommerce.Shared.Abstractions.Modules;
using System.Reflection;

namespace Ecommerce.Bootstrapper
{
    internal static class ModuleLoader
    {
        public static IList<Assembly> GetAssemblies()
        {
            const string modulePart = "Ecommerce.Modules.";
            //var assemblies = new List<Assembly>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            var filesDirectory = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                .Where(f => f.Contains(modulePart))
                .ToList();
            foreach (var fileDirectory in filesDirectory)
            {
                assemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(fileDirectory)));
            }
            return assemblies;
        }
        public static IList<IModule> LoadModules(IList<Assembly> assemblies)
        {
            return assemblies
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(IModule).IsAssignableFrom(x) && !x.IsInterface)
                .Select(x => Activator.CreateInstance(x))
                .Cast<IModule>()
                .ToList();
        }
    }
}
