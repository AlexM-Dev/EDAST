using EDAST.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EDAST.Core {
    public interface IAddon {
        /// <summary>
        /// The name of the addon.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Whether a configuration file should be loaded
        /// for the addon or not.
        /// </summary>
        bool UseConfig { get; }

        bool DoesProcessAddress { get; }

        /// <summary>
        /// Initialise the addon with the appropriate manager
        /// and configuration file, if applicable.
        /// </summary>
        /// <param name="manager">The manager to use.</param>
        /// <param name="conf">The configuration file to use.</param>
        /// <returns></returns>
        Task<bool> InitialiseAsync(Manager manager, object conf);

        /// <summary>
        /// Sends a shutdown signal to the addon.
        /// </summary>
        Task ShutdownAsync();

        /// <summary>
        /// Processes an address to determine if successful or not.
        /// </summary>
        /// <param name="addr">The address to check.</param>
        /// <returns>Result of the check.</returns>
        Task<AddressResult> ProcessAddressAsync(Address addr);

        /// <summary>
        /// Reads a result from checking an address.
        /// </summary>
        /// <param name="result">The result to read from.</param>
        Task ReadResultAsync(AddressResult result);

        /// <summary>
        /// Reads all results from a series of checks of addresses.
        /// </summary>
        /// <param name="results">The results to read from.</param>
        Task ReadResultsAsync(params AddressResult[] results);

        /// <summary>
        /// Process messages sent from another addon.
        /// </summary>
        /// <param name="source">The source addon from which the data was sent.</param>
        /// <param name="data">The data to be sent.</param>
        /// <returns>The result of the message.</returns>
        Task<object> ProcessDataAsync(IAddon source, object data);
    }
}
