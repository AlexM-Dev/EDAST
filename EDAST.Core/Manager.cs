using EDAST.Core.Data;
using EDAST.Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace EDAST.Core {
    /// <summary>
    /// Facilitates extensibility and inter-addon
    /// communication for the program.
    /// </summary>
    public class Manager {
        #region Events



        #endregion

        #region Properties

        public List<Address> Addresses { get; }

        #endregion

        #region Fields 

        private List<IAddon> addons;
        private string addrPath;
        private string confPath;

        #endregion

        public Manager(string confPath,
            List<Address> addresses, List<IAddon> addons) {

            this.confPath = confPath;
            this.Addresses = addresses;
            this.addons = addons;
        }

        /// <summary>
        /// Load each address from a given path.
        /// </summary>
        /// <param name="path">The path to load from.</param>
        public async Task<bool[]> LoadAddresses(string path) {
            var addrFiles = Directory.GetFiles(addrPath);

            return await Task.WhenAll(addrFiles
                .Select(async a => {
                    try {
                        var addrData = await ConfLoader.LoadAsync(a,
                            new Address());

                        this.Addresses.Add(addrData);

                        return true;
                    } catch {
                        return false;
                    }
                }));
        }

        /// <summary>
        /// Initialises all addons which were located
        /// by the program itself. This sets their manager
        /// to the current instance of this class, and loads
        /// their default configuration files.
        /// </summary>
        /// <returns></returns>
        public async Task<bool[]> InitialiseAddons() {
            return await Task.WhenAll(addons
                .Select(async a => await a.InitialiseAsync(this, 
                    a.UseConfig ? await getConfig(a) : null)));
        }

        private async Task<object> getConfig(IAddon source) {
            var conf = Path.Combine(confPath, source.Name + ".json");

            try {
                var confData = await ConfLoader.LoadAsync(conf, 
                    new object());

                return confData;
            } catch {
                return null;
            }
        }

        public async Task Log(IAddon source, string message) {

        }

        /// <summary>
        /// Sends data to all addons with a matching name.
        /// </summary>
        /// <param name="source">The source addon.</param>
        /// <param name="dest">The destination addon name (Regex).</param>
        /// <param name="data">The data to send.</param>
        /// <returns>The returned data from each addon.</returns>
        public async Task<object[]> SendAsync(IAddon source, string dest, object data) {
            return await Task.WhenAll(addons
                .Where(a => Regex.IsMatch(source.Name, dest))
                .Select(async a => await a.ProcessDataAsync(source, data)));
        }
    }
}
