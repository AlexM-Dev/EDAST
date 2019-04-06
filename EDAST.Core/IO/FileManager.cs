using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using EDAST.Core.Data;

namespace EDAST.Core.IO {
    public class FileManager {
        public string ConfigPath { get; set; }
        public string AddressPath { get; set; }

        private Manager manager;

        public FileManager(Manager m) {
            this.manager = m;
        }

        public async Task LoadAddonsConfigAsync() {
            var invalidChars = Path.GetInvalidPathChars();

            if (!Directory.Exists(ConfigPath))
                Directory.CreateDirectory(ConfigPath);

            foreach (var data in manager.AddonData) {
                object conf = null;

                if (data.Key.UseConfig) {
                    var confName = new StringBuilder(data.Key.Name);

                    foreach (var c in invalidChars)
                        confName = confName.Replace(c, '_');

                    string confPath = Path.Combine(ConfigPath,
                        confName.ToString() + ".json");

                    conf = await ConfManager.LoadAsync<object>(confPath, null);
                } else {
                    conf = null;
                }

                manager.AddonData[data.Key] = conf;
            }
        }

        public async Task<bool> SaveConfigAsync<T>(IAddon source, T data) {
            var conf = Path.Combine(ConfigPath, source.Name + ".json");

            try {
                var confData = await data.SaveAsync(conf);
            } catch {
                return false;
            }

            return true;
        }

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
