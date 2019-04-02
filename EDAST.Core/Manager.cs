using EDAST.Core.Data;
using EDAST.Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using EDAST.Core.Events;

namespace EDAST.Core {
    /// <summary>
    /// Facilitates extensibility and inter-addon
    /// communication for the program.
    /// </summary>
    public class Manager {
        #region Events

        public event EventHandler AddonsInitialised = (o, e) => { };
        public event EventHandler<CheckingEventArgs> Checking = (o, e) => { };
        public event EventHandler<FinishedEventArgs> Checked = (o, e) => { };

        #endregion

        #region Properties

        public List<Address> Addresses { get; }
        public List<IAddon> Addons { get; }

        #endregion

        #region Fields 

        private string addrPath;
        private string confPath;

        #endregion

        public Manager(string addrPath, string confPath) {
            this.addrPath = addrPath;
            this.confPath = confPath;
            this.Addresses = new List<Address>();
            this.Addons = new List<IAddon>();
        }

        /// <summary>
        /// Load each address from the addresses path.
        /// </summary>
        public async Task<bool[]> LoadAddresses() {
            if (!Directory.Exists(addrPath)) {
                Directory.CreateDirectory(addrPath);
                return new bool[0];
            }

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
            var result = await Task.WhenAll(Addons
                .Select(async a => await a.InitialiseAsync(this,
                    a.UseConfig ? await getConfig(a) : null)));

            AddonsInitialised(this, EventArgs.Empty);

            return result;
        }

        public async Task<bool> SaveConfig<T>(IAddon source, T data) {
            var conf = Path.Combine(confPath, source.Name + ".json");

            try {
                var confData = await data.SaveAsync(conf);
            } catch {
                return false;
            }

            return true;
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
            return await Task.WhenAll(Addons
                .Where(a => Regex.IsMatch(source.Name, dest))
                .Select(async a => await a.ProcessDataAsync(source, data)));
        }

        public AddressResult CheckAddress(Address addr) {
            var result = new AddressResult(addr);

            this.Checking(this, new CheckingEventArgs(result));

            return result;
        }

        public IEnumerable<AddressResult> CheckAddresses(IEnumerable<Address> addresses) {
            var results = addresses.Select(addr => CheckAddress(addr));

            this.Checked(this, new FinishedEventArgs(results));

            return results;
        }
    }
}
