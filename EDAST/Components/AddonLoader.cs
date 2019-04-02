using EDAST.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDAST.Components {
    internal class AddonLoader {
        private string addonPath;

        public List<IAddon> LoadedAddons { get; }
        
        public AddonLoader(string addonPath) {
            this.addonPath = addonPath;
        }


    }
}
