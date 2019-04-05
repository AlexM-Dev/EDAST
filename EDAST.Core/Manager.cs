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
        public async Task<Dictionary<IAddon, bool>> InitialiseAddonsAsync() {
            // Attempts to initialise each addon.
            // Returns statuses of each addon.
            var result = await Task.WhenAll(AddonData
                .Select(async a => (a.Key, await a.Key.InitialiseAsync(this,
                    a.Key.UseConfig ? a.Value : null))));

            AddonsInitialised(this, EventArgs.Empty);

            // Convert array result to dictionary.
            return result.ToDictionary(x => x.Key, x => x.Item2);
        }

        /// <summary>
        /// Sends a shutdown signal to all addons.
        /// </summary>
        /// <returns>Whether each addon was successful in shutting down.</returns>
        public async Task<Dictionary<IAddon, bool>> ShutdownAddonsAsync() {
            // Wait for each addon to receive the shutdown signal.
            var result = await Task.WhenAll(AddonData
                .Select(async a => (a.Key, await a.Key.ShutdownAsync())));

            // Return success/not.
            return result.ToDictionary(x => x.Key, x => x.Item2);
        }

        public async Task<AddressResult> ProcessAddressAsync(Address addr) {
            // Send processing requests to each addon.
            var result = await Task.WhenAll(AddonData
                .Where(a => a.Key.DoesProcessAddress)
                .Select(async a => await a.Key.ProcessAddressAsync(addr)));

            // Process each addon's result and merge them.
            var finalResult = AddressResult.Merge(result);

            // Allow each addon to read the final result of the address.
            await Task.WhenAll(AddonData
                .Select(a => a.Key.ReadResultAsync(finalResult)));

            // Return the processed result.
            return finalResult;
        }

        public Task<AddressResult[]> ProcessAddressesAsync(params Address[] addr) {
            var result = Task.WhenAll(addr
                .Select(async a => await ProcessAddressAsync(a));

            return result;
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
