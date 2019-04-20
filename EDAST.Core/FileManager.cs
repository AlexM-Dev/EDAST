using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using EDAST.Core.Data;
using EDAST.Core.IO;

namespace EDAST.Core {
    public class FileManager {
        public string ConfigPath { get; set; }
        public string AddressPath { get; set; }

        private Manager manager;

        public FileManager(Manager m) {
            this.manager = m;
        }

        /// <summary>
        /// Loads configuration for each addon.
        /// </summary>
        public async Task LoadAddonsConfigAsync() {
            // Get all invalid characters for the system.
            var invalidChars = Path.GetInvalidPathChars();

            // Create the config path if non-existent. 
            if (!Directory.Exists(ConfigPath))
                Directory.CreateDirectory(ConfigPath);

            // Enumerate through each addon.
            foreach (var addon in manager.AddonData.Keys.ToList()) {
                object conf = null;

                // Check if the addon uses a configuration file.
                if (addon.UseConfig) {
                    // Replace all invalid characters within the addon name.
                    var confName = new StringBuilder(addon.Name);

                    foreach (var c in invalidChars)
                        confName = confName.Replace(c, '_');

                    // Get the config file.
                    string confPath = Path.Combine(ConfigPath,
                        confName.ToString() + ".json");

                    conf = await ConfManager.LoadAsync<object>(confPath, null);
                } else {
                    conf = null;
                }

                // Set the config for the addon.
                manager.AddonData[addon] = conf;
            }
        }

        /// <summary>
        /// Saves an addon's config file under the config path.
        /// </summary>
        /// <typeparam name="T">The type of data to save.</typeparam>
        /// <param name="source">The addon source.</param>
        /// <param name="data">The data to save.</param>
        /// <returns></returns>
        public async Task<bool> SaveConfigAsync<T>(IAddon source, T data) {
            var conf = Path.Combine(ConfigPath, source.Name + ".json");

            try {
                var confData = await data.SaveAsync(conf);
            } catch {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Loads a set of addresses from the AddressPath in the current instance.
        /// </summary>
        public async Task LoadAddressesAsync() {
            if (!Directory.Exists(AddressPath))
                Directory.CreateDirectory(AddressPath);

            foreach (var addrFile in Directory.GetFiles(AddressPath,
                "*.json")) {
                var address = await ConfManager.LoadAsync(addrFile,
                    new Address());

                manager.Addresses.Add(address);
            }
        }
    }
}
