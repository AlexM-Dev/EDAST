using System;
using System.Collections.Generic;
using System.Text;

namespace EDAST.Components {
    internal class Config {
        public string AddonsPath { get; set; }
        public string AddonsFormat { get; set; }
        public string AddressesPath { get; set; }
        public string ConfigPath { get; set; }
        public int Interval { get; set; }

        public Config() {
            this.AddonsPath = "addons";
            this.AddonsFormat = "EDAST.*.dll";
            this.AddressesPath = "addresses";
            this.ConfigPath = "config";
            this.Interval = 30;
        }
    }
}
