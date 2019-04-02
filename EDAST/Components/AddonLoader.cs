using EDAST.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace EDAST.Components {
    /// <summary>
    /// Provides a manager with addons to use and load.
    /// </summary>
    internal class AddonLoader {
        private Manager manager;
        private Config conf;

        public List<IAddon> LoadedAddons { get; }
        
        public AddonLoader(Manager manager, Config conf) {
            this.manager = manager;
            this.conf = conf;
        }

        public void LoadAddons() {
            var files = getFiles();
            var addons = getAddons(files);

            manager.Addons.AddRange(addons);
        }

        private string[] getFiles() {
            var fullPath = Path.GetFullPath(conf.AddonsPath);

            if (!Directory.Exists(conf.AddonsPath)) {
                Directory.CreateDirectory(conf.AddonsPath);
            }

            return Directory.GetFiles(fullPath, conf.AddonsFormat, 
                SearchOption.AllDirectories);
        }

        private IEnumerable<IAddon> getAddons(string[] assemblies) {
            return assemblies.SelectMany(path => {
                var assembly = loadAssembly(path);
                return createAddons(assembly);
            });
        }

        private Assembly loadAssembly(string path) {
            string fullPath = Path.GetFullPath(path);

            var context = new AddonLoadContext(fullPath);

            return context.LoadFromAssemblyName(new 
                AssemblyName(Path.GetFileNameWithoutExtension(path)));
        }

        private IEnumerable<IAddon> createAddons(Assembly assembly) {
            foreach (var type in assembly.GetTypes()) {
                if (typeof(IAddon).IsAssignableFrom(type)) {
                    var addon = Activator.CreateInstance(type) as IAddon;

                    if (addon != null)
                        yield return addon;
                }
            }
        }
    }
}
