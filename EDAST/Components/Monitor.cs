using EDAST.Core;
using EDAST.Core.Data;
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
            this.interval = (double)interval / 1000;

            timer = new Timer(this.interval);
            timer.Start();
        }

        private async void monitorElapsed(object sender, ElapsedEventArgs e) {

        }

        private async bool isUp(Address addr) {

        }
    }
}
