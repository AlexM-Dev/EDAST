using EDAST.Components;
using EDAST.Core;
using EDAST.Core.Helpers;
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
            this.conf = await ConfLoader.LoadAsync("config.json",
                new Config());

            // Manager for addons.
            this.manager = new Manager(this.conf.AddressesPath, 
				this.conf.ConfigPath);
            await this.manager.LoadAddresses();
			
            // Addons loader.
            this.loader = new AddonLoader(manager, conf);
            this.loader.LoadAddons();
            await this.manager.InitialiseAddons();

            // EDAST Monitor.
            this.monitor = new Monitor(manager, this.conf.Interval);
            monitor.Start();

            return true;
        }

        static void Main(string[] args) =>
            new Program().MainAsync(args).Wait();

    }
}
