using EDAST.Core.Data;
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

        public event EventHandler AddonsInitialised = (o, e) => { };

        #endregion

        #region Properties

        public Dictionary<IAddon, object> AddonData { get; }
        public List<Address> Addresses { get; }
        public FileManager FileManager { get; }

        #endregion

        public Manager() : this(new List<Address>(),
            new Dictionary<IAddon, object>()) { }

        public Manager(List<Address> addresses,
            Dictionary<IAddon, object> addonData) {

            this.Addresses = addresses;
            this.AddonData = addonData;

            this.FileManager = new FileManager(this);
        }

        /// <summary>
        /// Initialises all addons which were located
        /// by the program itself. This sets their manager
        /// to the current instance of this class, and loads
        /// their default configuration files.
        /// </summary>
        /// <returns>Whether each addon is successful in initialising.</returns>
        public async Task<Dictionary<IAddon, bool>> InitialiseAddonsAsync() {
            // Attempts to initialise each addon.
            // Returns statuses of each addon.
            var result = await Task.WhenAll(AddonData
                .Select(async a => {
                    try {
                        var init = await a.Key.InitialiseAsync(this,
                            a.Key.UseConfig ? a.Value : null);
                        return (a.Key, init);
                    } catch {
                        return (a.Key, false);
                    }
                }));

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

        /// <summary>
        /// Sends a request to each addon to process an address.
        /// </summary>
        /// <param name="addr">The address to process.</param>
        /// <returns>The processed AddressResult.</returns>
        public async Task<AddressResult> ProcessAddressAsync(Address addr) {
            // Send processing requests to each addon.
            var result = await Task.WhenAll(AddonData
                .Where(a => a.Key.DoesProcessAddress)
                .Select(async a => {
                    try {
                        return await a.Key.ProcessAddressAsync(addr);
                    } catch {
                        return new AddressResult(addr) {
                            ProcessErrorOccurred = true
                        };
                    }
                }));

            // Process each addon's result and merge them.
            var finalResult = AddressResult.Merge(result);

            // Allow each addon to read the final result of the address.
            await Task.WhenAll(AddonData
                .Select(async a => {
                    try {
                        await a.Key.ReadResultAsync(finalResult);
                    } catch { }
                }));

            // Return the processed result.
            return finalResult;
        }

        /// <summary>
        /// Sends requests to each addon to process a series of addresses.
        /// </summary>
        /// <param name="addr">The addresses to process.</param>
        /// <returns>The processed AddressResults.</returns>
        public Task<AddressResult[]> ProcessAddressesAsync() {
            var result = Task.WhenAll(Addresses
                .Select(async a => await ProcessAddressAsync(a)));

            return result;
        }

        /// <summary>
        /// Sends a message/data to another addon(s).
        /// </summary>
        /// <param name="source">The source addon.</param>
        /// <param name="name">The name (regex) of the addons(s).</param>
        /// <param name="data">The data to send.</param>
        /// <returns>Data returned from each addon.</returns>
        public async Task<Dictionary<IAddon, object>> SendDataAsync(IAddon source,
            string name, object data) {

            var result = await Task.WhenAll(AddonData
                .Where(a => Regex.IsMatch(source.Name, name))
                .Select(async a => {
                    try {
                        var proc = await a.Key.ProcessDataAsync(source, data);
                        return (a.Key, proc);
                    } catch {
                        return (a.Key, null);
                    }
                }));

            return result.ToDictionary(x => x.Key, x => x.Item2);
        }

        public async Task LogAsync(IAddon source, string message) {

        }
    }
}
