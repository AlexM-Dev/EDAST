using EDAST.Components;
using EDAST.Core;
using EDAST.Core.IO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EDAST {
    partial class Program {
        private Manager manager;
        private Monitor monitor;
        private Config conf;
        private AddonLoader loader;

        async Task<bool> initAsync() {
            // Configuration file for the program.
            this.conf = await ConfManager.LoadAsync("config.json",
                new Config());

            /* Manager */
            this.manager = new Manager();

            // Addons loader.
            this.loader = new AddonLoader(manager, conf);
            this.loader.LoadAddons();

            /* Manager -> Addresses */
            this.manager.FileManager.AddressPath = this.conf.AddressesPath;
            await this.manager.FileManager.LoadAddressesAsync();

            /* Manager -> Addons & Configuration */
            this.manager.FileManager.ConfigPath = this.conf.ConfigPath;
            await this.manager.FileManager.LoadAddonsConfigAsync();
            await this.manager.InitialiseAddonsAsync();

            // EDAST Monitor.
            this.monitor = new Monitor(manager, this.conf.Interval);
            monitor.Start();

            return true;
        }

        static void Main(string[] args) =>
            new Program().MainAsync(args).Wait();

    }
}
