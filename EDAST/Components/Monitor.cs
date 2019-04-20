using EDAST.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace EDAST.Components {
    internal class Monitor {
        private Manager manager;
        private double interval;

        private Timer timer;

        public Monitor(Manager manager, int interval) {
            this.manager = manager;
            this.interval = (double)interval * 1000;

            timer = new Timer(this.interval);
            timer.Elapsed += monitorElapsed;
        }

        /// <summary>
        /// Starts the monitor.
        /// </summary>
        public void Start() {
            timer.Start();
        }

        private async void monitorElapsed(object sender, ElapsedEventArgs e) {
            var result = await manager.ProcessAddressesAsync();
            Console.WriteLine("Performed check. " + result.Length);
        }
    }
}
