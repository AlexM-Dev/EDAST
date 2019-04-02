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

        /// <summary>
        /// Initialise the addon with the appropriate manager
        /// and configuration file, if applicable.
        /// </summary>
        /// <param name="manager">The manager to use.</param>
        /// <param name="conf">The configuration file to use.</param>
        /// <returns></returns>
        Task<bool> InitialiseAsync(Manager manager, object conf);

        /// <summary>
        /// Process messages sent from another addon.
        /// </summary>
        /// <param name="source">The source addon from which the data was sent.</param>
        /// <param name="data">The data to be sent.</param>
        /// <returns>The result of the message.</returns>
        Task<object> ProcessDataAsync(IAddon source, object data);

        /// <summary>
        /// Sends a shutdown signal to the addon.
        /// </summary>
        void Shutdown();
    }
}
