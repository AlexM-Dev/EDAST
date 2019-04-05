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

        #endregion

        #region Properties

        public Dictionary<IAddon, object> AddonData { get; }
        public List<Address> Addresses { get; }

        #endregion

        public Manager() {
            this.Addresses = new List<Address>();
            this.AddonData = new Dictionary<IAddon, object>();
        }

        /// <summary>
        /// Initialises all addons which were located
        /// by the program itself. This sets their manager
        /// to the current instance of this class, and loads
        /// their default configuration files.
        /// </summary>
        /// <returns></returns>
        public async Task<bool[]> InitialiseAddonsAsync() {
            var result = await Task.WhenAll(AddonData
                .Select(async a => await a.Key.InitialiseAsync(this,
                    a.Key.UseConfig ? a.Value : null)));

            AddonsInitialised(this, EventArgs.Empty);

            return result;
        }

        public async Task<bool[]> ShutdownAddonsAsync() {
            var result = await Task.WhenAll(AddonData
                .Select(async a => await a.Key.ShutdownAsync()));

            return result;
        }

        public Task<AddressResult> ProcessAddressAsync(Address addr) {

        }

        public Task<AddressResult[]> ProcessAddressesAsync(Address addr) {

        }

        public Task<object[]> SendDataAsync(string nameRegex, object data) {

        }

        public async Task LogAsync(IAddon source, string message) {

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

        /// <summary>
        /// Begin checking of addresses using all available addons.
        /// </summary>
        /// <param name="addresses">The addresses to check.</param>
        /// <returns></returns>
        public IEnumerable<AddressResult> CheckAddresses() {
            var results = Addresses.Select(addr => CheckAddress(addr));

            this.Checked(this, new FinishedEventArgs(results));

            return results;
        }
    }
}
