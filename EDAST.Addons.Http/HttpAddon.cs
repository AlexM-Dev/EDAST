using EDAST.Core;
using EDAST.Core.Data;
using System;
using System.Threading.Tasks;

namespace EDAST.Addons.Http {
    public class HttpAddon : IAddon {
        public string Name => "HTTP Checks for EDAST";

        public bool UseConfig => true;

        public bool DoesProcessAddress => true;

        public async Task<bool> InitialiseAsync(Manager manager, object conf) {
            manager.AddonsInitialised += this.Manager_AddonsInitialised;
            Console.WriteLine("Hello");
            return true;
        }

        private void Manager_AddonsInitialised(object sender, EventArgs e) {
            Console.WriteLine("Yeeted..");
        }

        public async Task<object> ProcessDataAsync(IAddon source, object data) {
            Console.WriteLine("Data processed.");
            return true;
        }

        public async Task<bool> ShutdownAsync() {
            return false;
        }

        public async Task<AddressResult> ProcessAddressAsync(Address addr) {
            Console.WriteLine("A check http addon");
            return new AddressResult(addr);
        }

        public async Task ReadResultAsync(AddressResult result) {

        }

        public async Task ReadResultsAsync(params AddressResult[] results) {

        }
    }
}
