using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using EDAST.Core.Data;

namespace EDAST.Core.IO {
    public static class FileManager {
        public static async Task LoadAddonDataAsync(this Manager manager,
            string path) {

            var invalidChars = Path.GetInvalidPathChars();

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            foreach (var data in manager.AddonData) {
                object conf = null;

                if (data.Key.UseConfig) {
                    var confName = new StringBuilder(data.Key.Name);

                    foreach (var c in invalidChars)
                        confName = confName.Replace(c, '_');

                    string confPath = Path.Combine(path,
                        confName.ToString() + ".json");

                    conf = await ConfManager.LoadAsync<object>(confPath, null);
                } else {
                    conf = null;
                }

                manager.AddonData[data.Key] = conf;
            }
        }

        public static async Task LoadAddressesAsync(this Manager manager,
            string path) {

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            foreach (var addrFile in Directory.GetFiles(path, "*.json")) {
                var address = await ConfManager.LoadAsync(addrFile,
                    new Address());

                manager.Addresses.Add(address);
            }
        }
    }
}
