using EDAST.Core.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EDAST.Core {
    /// <summary>
    /// Facilitates extensibility and inter-addon
    /// communication for the program.
    /// </summary>
    public class Manager {
        #region Events



        #endregion

        #region Properties

        public List<Address> Addresses { get; private set; }

        #endregion

        #region Fields 

        private List<IAddon> addons;
        private string addrPath;
        private string confPath;
        
        #endregion

        public Manager(string addrPath, string confPath, 
            List<Address> addresses, List<IAddon> addons) {

            this.addrPath = addrPath;

            // Load the addresses.

            this.confPath = confPath;
            this.Addresses = addresses;
            this.addons = addons;
        }

        public async Task Log(IAddon source, string message) {

        }

        public async Task<object> SendMessageAsync(IAddon source, object data) {
            return await Task.FromResult<object>(default);
        }

        public async Task<T> GetConfig<T>(IAddon source) {
            var conf = Path.Combine(confPath, source.Name + ".json");

            return await Task.FromResult<T>(default);
        }
    }
}
